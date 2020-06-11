using System.Linq;
using UnityEditor;

namespace blProject.scripts.EditorFramework.Controllers
{
    public class HorizontalBlock 
        : BaseBlock
    {
        public HorizontalBlock() 
            : base()
        {
        }
        
        public HorizontalBlock(IController[] controllers) 
            : base(controllers)
        {
        }

        public override void OnGUI()
        {
            if (_controllers == null || !_controllers.Any())
            {
                return;
            }
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i].OnGUI();
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}