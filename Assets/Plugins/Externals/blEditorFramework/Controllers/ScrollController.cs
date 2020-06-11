using UnityEngine;

namespace blProject.scripts.EditorFramework.Controllers
{
    public class ScrollController<TItemController> 
        : IController where TItemController : IController, new()
    {
        private IController[] _items;
        
        public ScrollController()
        {
        }

        public void OnGUI()
        {
        }
    }
}