using System;
using System.Linq;
using UnityEngine;

namespace WAL.PrefabLightmapping
{
    public static class Utilities
    {
        public static string FullToProjectPath(string path)
        {
            string[] parts = path.Split('/');
            int from = Array.FindIndex(parts, 0, parts.Length, s => s == "Assets");

            return string.Join("/", parts.Skip(from));
        }

        public static GameObject CopyObjectHierarchy(Transform target)
        {
            return CopyObjectHierarchyStep(target, null);
        }

        private static GameObject CopyObjectHierarchyStep(Transform target, Transform parent)
        {
            GameObject copied = new GameObject(target.name);
            Transform copiedTransform = copied.transform;
            copiedTransform.SetParent(parent);
            copiedTransform.localPosition = target.localPosition;
            copiedTransform.localRotation = target.localRotation;

            foreach (Component c in target.GetComponents<Component>())
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(c);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(copied);
            }

            foreach (Transform t in target)
            {
                CopyObjectHierarchyStep(t, copiedTransform);
            }

            return copied;
        }
    }
}