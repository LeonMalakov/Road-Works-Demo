using System;
using UnityEditor;
using UnityEngine;

namespace WAL.Core
{
    [CustomPropertyDrawer(typeof(EnumMatchingBase), true)]
    public class EnumMatchingPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Validation.

            // Get array property.
            SerializedProperty array = property.FindPropertyRelative("_elements");

            // If Property is not serializable, exit.
            if (array == null)
            {
                return;
            }

            // Setup EnumMatching array to correspond it's enum.
            SetupWithEnums(property, array, out Type enumType);


            // Output below.

            // Foldout label-button.
            position.y += 3;
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(new Rect(position.x, position.y, position.width, 22), property.isExpanded, label);
            EditorGUI.EndFoldoutHeaderGroup();
            position.y += 22;


            // If property is expanded (i.e. foldout opened).
            if (property.isExpanded)
            {
                // Draw each array element.
                string[] enumNames = enumType.GetEnumNames();
                for (int i = 0; i < array.arraySize; i++)
                {
                    // Draw it's self property drawer for property.
                    EditorGUI.PropertyField(position, array.GetArrayElementAtIndex(i), new GUIContent(enumNames[i]), true);
                    position.y += EditorGUI.GetPropertyHeight(array.GetArrayElementAtIndex(i));
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Height of it's element.
            float height = 0;

            // Array elements
            if (property.isExpanded)
            {
                SerializedProperty array = property.FindPropertyRelative("_elements");
                for (int i = 0; i < array.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(array.GetArrayElementAtIndex(i));
                }

                // Margin before next Property in inspector.
                height += 15;
            }

            // Margin before prev Property in inspector.
            height += 3;

            // Label.
            height += 22;

            return height;
        }

        private void SetupWithEnums(SerializedProperty property, SerializedProperty array, out Type enumType)
        {
            // Type of serialized property (EnumMatching).
            Type type = GetSerializedPropertyType(property);

            // Get <TEnum> type of EnumMatching<TEnum, TData>.
            enumType = type.GetGenericArguments()[0];

            // Find count of values in EnumType (skip values < 0 AND values with name 'Total').
            int count = 0;
            foreach (int i in enumType.GetEnumValues())
                if (i >= 0 && enumType.GetEnumNames()[i] != "Total") count++;

            // Compare array size and enum values count.
            // Add or remove if not the same.
            if (array.arraySize != count)
            {
                while (array.arraySize < count)
                    array.InsertArrayElementAtIndex(array.arraySize);

                while (array.arraySize > count)
                    array.DeleteArrayElementAtIndex(array.arraySize - 1);
            }
        }

        // Gets serialized property type (reflection).
        private Type GetSerializedPropertyType(SerializedProperty property)
        {
            // SerializedObject.
            UnityEngine.Object context = property.serializedObject.targetObject;
            Type type = context.GetType();

            // Property field.
            ReflectionUtilities.GetFieldInfoFromPropertyPath(type, property.propertyPath, out Type t);

            return t;
        }

    }
}