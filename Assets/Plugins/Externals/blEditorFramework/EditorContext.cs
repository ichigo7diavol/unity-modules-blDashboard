using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using blExternals.blBind;
using blExternals.blEditorFramework.UIElements;
using blExternals.blReflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

namespace blProject.scripts.EditorFramework
{
    [InitializeOnLoad]
    public static class EditorContext
    {
        private const string _errorLine = "Context already contains {0}!";
        private const string _resetLine = "Reset";
        
        private const string _editorCacheLine = ".editorCache";
        private const string _modelsCacheLine = "models";
        
        private static Dictionary<Type, TestModel> _windows;
        private static string _projectPath = Path.GetDirectoryName(Application.dataPath);

        private static DirectoryInfo _cacheDirectory;
        private static FileInfo _hierarchyFile;

        private static JsonSerializer _serializer;

        private static JsonSerializer Serializer 
            => _serializer ?? (_serializer = new JsonSerializer());
        
        static EditorContext()
        {
            _windows = new Dictionary<Type, TestModel>();
            
            Debug.Log("Context created!");
            Init();
        }

        private static void Init()
        {
            CheckCacheDirectory();
            CheckCacheStructure();
        }

        private static void CheckCacheDirectory()
        {
            _cacheDirectory = Directory
                .CreateDirectory($"{_projectPath}{Path.DirectorySeparatorChar}{_editorCacheLine}");
        }

        private static void CheckCacheStructure()
        {
            var filePath = $"{_cacheDirectory.FullName}{Path.DirectorySeparatorChar}{_modelsCacheLine}";

            _hierarchyFile = new FileInfo(filePath);
            
            if (_hierarchyFile.Exists)
            {
                LoadEntitiesHierarchy();
            }
        }

        [MenuItem(WindowsRouter.EditorName + WindowsRouter.PathSeparator + "Save")]
        private static void SaveEntitiesHierarchy()
        {
            using (var stream = new StreamWriter(_hierarchyFile.OpenWrite()))
            {
                Serializer.Serialize(stream, _windows);
            }
        }
        
        [MenuItem(WindowsRouter.EditorName + WindowsRouter.PathSeparator + "Load")]
        private static void LoadEntitiesHierarchy()
        {
            if (_hierarchyFile == null)
            {
                return;
            }
            using (var stream = new StreamReader(_hierarchyFile.OpenRead()))
            {
                var reader = new JsonTextReader(stream);
                
                _windows = Serializer.Deserialize<Dictionary<Type, TestModel>>(reader);
            }
        }

        [MenuItem(WindowsRouter.EditorName + WindowsRouter.PathSeparator + _resetLine)]
        public static void Reset()
        {
            var sb = new StringBuilder();
            var model = new TestModel();

            var binds = new List<IBind>();
            var fields = new List<VisualElement>();
            
            var bindsFactory = new BindFactory();
            var fieldsFactory = new BaseBindableFieldFactory();
            
            foreach (var propertyInfo in typeof(TestModel)
                .GetMembers<SerializeAttribute>(isRecursive : true))
            {
                try
                {
                    binds.Add(bindsFactory.Create(model, propertyInfo));
                    fields.Add(fieldsFactory.Create(binds.Last())); 
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.Message }\n{e.StackTrace}");
                }
            }
            binds.ForEach(b 
                =>
            {
                var old = b.BoxedValue;
                b.SafeBoxedValue = Random.Range(int.MinValue, int.MaxValue);

                sb.AppendLine($"old => {old}; new => {b.SafeBoxedValue}");
            });
            Debug.Log(sb);            
            Init();
        }

        public static TWindowType AddWindow<TWindowType>()
            where TWindowType : BaseWindow 
        {
            var type = typeof(TWindowType);
            var window = EditorWindow.GetWindow<TWindowType>();

            window.Init();
            
            if (!_windows.ContainsKey(type))
            {
                ProcessModel(window.Model);
                
                _windows.Add(type, (TestModel)window.Model);
            }
            return window;
        }

        private static void ProcessModel(IModel model)
        {
        }
    }
}