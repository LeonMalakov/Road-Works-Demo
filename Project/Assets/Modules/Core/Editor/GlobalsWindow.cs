using UnityEditor;
using UnityEngine;

namespace WAL.Core
{
    public class GlobalsWindow : EditorWindow
    {

        private GlobalsSettings _settings;
          
        [MenuItem("WhyAmLocked/Window/Globals")]
        private static void ShowWindow()
        {
            GetWindow<GlobalsWindow>("Globals");
        }

        private void OnEnable()
        {
            _settings = GlobalsSettings.Load();
        }


        private void OnGUI()
        {
            GUILayout.Label("Settings", EditorStyles.boldLabel);

            SerializedObject serializedObject = new SerializedObject(_settings);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_settings.DataContainer)), true);

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}