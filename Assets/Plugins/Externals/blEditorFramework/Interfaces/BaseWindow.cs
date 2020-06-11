using UnityEditor;
using UnityEngine;

namespace blProject.scripts.EditorFramework
{
    public abstract class BaseWindow : EditorWindow
    {
        protected abstract string WindowName { get; }

        private IModel _model;

        public IModel Model => _model;
        
        /// <summary>
        /// Initialize window
        /// </summary>
        public virtual void Init()
        {
            var editor = EditorWindow.GetWindow(this.GetType());

            editor.titleContent = new GUIContent(WindowName);
            
            _model = new TestModel();
        }

        public virtual void OnGUI()
        {
        }
    }
}