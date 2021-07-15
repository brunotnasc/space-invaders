using UnityEngine;
using UnityEditor;

namespace SpaceInvaders.Editors
{
    [CustomEditor(typeof(UfoSpawner))]
    public class UfoSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Spawn"))
            {
                (target as UfoSpawner).Spawn(0f);
            }
        }
    }
}