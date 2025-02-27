using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Common;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.Editor
{
    /// <summary>
    /// Add "Refresh" button to IconCollection script
    /// </summary>
    [CustomEditor(typeof(IconCollection))]
    public class IconCollectionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var collection = (IconCollection) target;

            if (GUILayout.Button("Refresh"))
            {
				collection.Refresh();
            }
        }
    }
}