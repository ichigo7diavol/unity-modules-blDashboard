using blProject.scripts.EditorFramework.Extensions;
using UnityEngine;

namespace blProject.scripts.EditorFramework.Controllers
{
    public class TestBoxController : IController
    {
        private readonly GUILayoutOption[] _options; 
        
        public TestBoxController()
        {
            _options = GUILayoutHelper.CreateOptions(100, 100);
        }

        public void OnGUI()
        {
            GUILayout.Box(GUIContent.none, _options);
        }
    }
}