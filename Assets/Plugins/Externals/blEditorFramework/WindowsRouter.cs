using UnityEditor;

namespace blProject.scripts.EditorFramework
{
    public static class WindowsRouter
    {
        public const string EditorName = "blEditor";
        public const string PathSeparator = "//";

        [MenuItem(EditorName + PathSeparator + "Editor")]
        public static void ShowMain()
        {
            ShowWindow<Editor>();
        }

        [MenuItem(EditorName + PathSeparator + "Debugger")]
        public static void ShowDebugger()
        {
            ShowWindow<Debugger>();
        }
        
        private static void ShowWindow<TWindowType>() 
            where TWindowType : BaseWindow
        {
            var window = EditorContext.AddWindow<TWindowType>();
            
            window.Show();
        }
    }
}