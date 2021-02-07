using System.Collections.Generic;
using UnityEngine;

namespace WAL.Core
{
    [DefaultExecutionOrder(-1)]
    public class SceneDefinition : MonoBehaviour
    {
        private void Awake()
        {
            // Find all scene dependencies.
            SceneContainerDependencies[] dependencies = FindObjectsOfType<SceneContainerDependencies>();
            List<Component> components = new List<Component>();
            for (int i = 0; i < dependencies.Length; i++)
            {
                components.AddRange(dependencies[i].Elements);
            }

            // Initialize Globals with scene dependencies.
            Globals.Initialize(components);
        }
    }
}