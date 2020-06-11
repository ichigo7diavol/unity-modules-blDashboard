using blProject.scripts.EditorFramework.Controllers;
using UnityEditor;
using UnityEngine;

namespace blProject.scripts.EditorFramework
{
    public class Debugger: BaseWindow
    {
        private const string Name = "Debugger";

        protected override string WindowName => Name;
        
        public override void Init()
        {
            base.Init();
        }
        
        public override void OnGUI()
        {
            base.OnGUI();
            
            CheckCurrentWindow();
            
        }

        private void CheckCurrentWindow()
        {
            
            
//            EditorGUI.LabelField(new Rect(0, 0, position.width,
//                EditorGUIUtility.singleLineHeight),
//                focusedWindow.position.ToString());

//            Group.DrawGroup(null);
        }

        private void Update()
        {
            Repaint();
        }
    }
}