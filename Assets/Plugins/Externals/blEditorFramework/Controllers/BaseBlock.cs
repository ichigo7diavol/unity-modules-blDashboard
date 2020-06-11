using System.Collections.Generic;
using System.Linq;

namespace blProject.scripts.EditorFramework.Controllers
{
    public abstract class BaseBlock : IController
    {
        protected IController[] _controllers;
        
        public BaseBlock()
        {
        }

        public BaseBlock(IController[] controllers)
        {
            _controllers = controllers;
        }

        public void AddControllers(IController[] controllers)
        {
            if (_controllers == null || !_controllers.Any())
            {
                _controllers = controllers;
            }
            else
            {
                _controllers.Concat(controllers);
            }
        }

        public abstract void OnGUI();
    }
}