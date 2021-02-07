using System;
using System.Collections.Generic;
using System.Reflection;

namespace WAL.Core
{
    public static class ReflectionUtilities
    {
        public static bool IsArrayOrList(this Type type)
        {
            // List.
            if (type == typeof(List<>))
                return true;

            // Array.
            return type.IsArray;
        }

        public static Type GetArrayOrListElementType(this Type type)
        {
            // List.
            if (type == typeof(List<>))
            {
                return type.GetGenericArguments()[0];
            }

            // Array.
            return type.GetElementType();
        }

        /// <summary>
        /// Straight from Unity's ScriptAttributeUtility. (<a href="https://github.com/Unity-Technologies/UnityCsReference/blob/73f36bfe71b68d241a6802e0396dc6d6822cb520/Editor/Mono/ScriptAttributeGUI/ScriptAttributeUtility.cs#L176">
        /// Link</a>)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldInfo GetFieldInfoFromPropertyPath(Type host, string path, out Type type)
        {
            FieldInfo field = null;
            type = host;
            string[] parts = path.Split('.');
            for (int i = 0; i < parts.Length; i++)
            {
                string member = parts[i];

                // Special handling of array elements.
                // The "Array" and "data[x]" parts of the propertyPath don't correspond to any types,
                // so they should be skipped by the code that drills down into the types.
                // However, we want to change the type from the type of the array to the type of the array element before we do the skipping.
                if (i < parts.Length - 1 && member == "Array" && parts[i + 1].StartsWith("data["))
                {
                    if (type.IsArrayOrList())
                        type = type.GetArrayOrListElementType();

                    // Skip rest of handling for this part ("Array") and the next part ("data[x]").
                    i++;
                    continue;
                }

                // GetField on class A will not find private fields in base classes to A,
                // so we have to iterate through the base classes and look there too.
                // Private fields are relevant because they can still be shown in the Inspector,
                // and that applies to private fields in base classes too.
                FieldInfo foundField = null;
                for (Type currentType = type; foundField == null && currentType != null; currentType = currentType.BaseType)
                    foundField = currentType.GetField(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (foundField == null)
                {
                    type = null;
                    return null;
                }

                field = foundField;
                type = field.FieldType;
            }
            return field;
        }
    }
}