using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEditor;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.Editor
{
    /// <summary>
    /// Add action buttons to LayerManager script
    /// </summary>
    [CustomEditor(typeof(LayerManager))]
    public class LayerManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (LayerManager) target;

            EditorGUILayout.LabelField("Service", EditorStyles.boldLabel);

            if (GUILayout.Button("Read Sorting Order"))
            {
                script.GetSpritesBySortingOrder();
            }

            if (GUILayout.Button("Set Sorting Order"))
            {
                script.SetSpritesBySortingOrder();
            }

            if (GUILayout.Button("Copy order"))
            {
                script.CopyOrder();
            }
        }
    }
}