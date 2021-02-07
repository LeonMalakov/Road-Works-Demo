using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WAL.Core
{
    [CustomEditor(typeof(GlobalsDataContainer))]
    public class GlobalsDataContainerEditor : Editor
    {
        private GlobalsDataContainer _target;
        private bool _isValid;
        private List<string> _notValidErrors = new List<string>();

        private GUIStyle ErrorHeaderStyle
        {
            get
            {
                GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
                s.normal.textColor = Color.red;
                s.fontSize = 14;
                return s;
            }
        }

        private void OnEnable()
        {
            _target = (GlobalsDataContainer)target;
            CheckValidity();
        }

        public override void OnInspectorGUI()
        {
            SerializedObject serializedObject = new SerializedObject(_target);

            // Output
            if (!_isValid)
            {
                GUILayout.Label("Data is not valid.", ErrorHeaderStyle);
                GUILayout.Label(" Errors list:");
                foreach (string s in _notValidErrors)
                {
                    GUILayout.Label($" - {s}");
                }
                GUILayout.Space(30);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Elements"), true);


            // Modifications handle.
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();

                CheckValidity();
            }
        }

        private void CheckValidity()
        {
            _isValid = true;
            _notValidErrors.Clear();

            if (_target.Elements != null)
            {
                Type[] types = _target.Elements.Select(e =>
                {
                    if (e == null) return null;
                    return e.GetType();
                }).ToArray();

                // Check if contains null or itself.
                if (!CheckContainsNullOrItself(types))
                    _isValid = false;

                // Check if contains two objects with the same type.
                if (!CheckContainsTheSameTypeObjects(types))
                    _isValid = false;
            }
        }

        private bool CheckContainsNullOrItself(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == null)
                {
                    _notValidErrors.Add("Contains NULL value.");
                    return false;
                }

                if (types[i] == typeof(GlobalsDataContainer))
                {
                    _notValidErrors.Add("Contains itself.");
                    return false;
                }
            }
            return true;
        }

        private bool CheckContainsTheSameTypeObjects(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
                for (int j = i + 1; j < types.Length; j++)
                    if (types[i] == types[j])
                    {
                        _notValidErrors.Add("Contains few objects with the same type.");
                        return false;
                    }

            return true;
        }
    }
}