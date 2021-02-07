using System;
using System.Collections.Generic;
using UnityEngine;

namespace WAL.Core
{
    internal static class GlobalsFactory
    {
        public static Globals Create()
        {
            // Setup object.
            Globals container = SetupContainerObject();

            // Load data object.
            GlobalsSettings data = GlobalsSettings.Load();

            // Create dictionary, if GlobalsDataContainer exists.
            Dictionary<Type, ScriptableObject> dataElementsDictionary = null;
            if (data.DataContainer != null)
                dataElementsDictionary = CreateDataElementsDictionary(data.DataContainer.Elements);

            // Unload data object.
           // UnloadSettings(data);

            // Construct GlobalsContainer instance.
            container.Construct(dataElementsDictionary);

            return container;
        }

        public static Dictionary<Type, T> CreateDataElementsDictionary<T>(ICollection<T> elements)
        {
            // Create dictionary
            Dictionary<Type, T> dataElements = new Dictionary<Type, T>(elements.Count);
            foreach (T s in elements)
            {
                Type sType = s.GetType();
                if (dataElements.ContainsKey(sType))
                {
                    throw new Exception("Dependencies Container can not contains elements with the same type.");
                }
                dataElements.Add(sType, s);
            }

            // Construct Container Instance.
            return dataElements;
        }

        private static Globals SetupContainerObject()
        {
            GameObject go = new GameObject("[Singleton] GlobalsContainer");
            UnityEngine.Object.DontDestroyOnLoad(go);
            return go.AddComponent<Globals>();
        }
    }
}