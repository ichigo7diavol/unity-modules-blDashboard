using UnityEngine;

namespace blProject.scripts.EditorFramework.Extensions
{
    public static class GUILayoutHelper
    {
        public static GUILayoutOption[] CreateOptions(int height, int width)
        {
            return new[] { GUILayout.Height(height), GUILayout.Width(width) };
        }
    }
}