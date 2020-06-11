using blProject.scripts.EditorFramework.Controllers;
using UnityEditor;
using UnityEngine;

namespace blProject.scripts.EditorFramework
{
    public class Editor : BaseWindow
    {
        private const string Name = "Base";

        protected override string WindowName => Name;

        private IController[] _items;
        private IController _item;
        
        public override void Init ()
        {
            base.Init();

            _items = new IController[]
            {                          
                new ScrollController<TestBoxController>(), 
                new ScrollController<TestBoxController>(), 
                new ScrollController<TestBoxController>(), 
            };
            _item = new HorizontalBlock(_items);
        }
        
        public override void OnGUI ()
        {
            base.OnGUI();
            
            _item.OnGUI();
            
            
        }
    }
    
}
