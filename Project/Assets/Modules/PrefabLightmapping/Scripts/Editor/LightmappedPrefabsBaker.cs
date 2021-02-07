using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WAL.PrefabLightmapping
{
    public class LightmappedPrefabsBaker : EditorWindow
    {
        [Serializable]
        public class Settings
        {
            [Header("Material with lightmap texture.")]
            public Material LightmappedMaterial;
        }

        private Settings _settings;

        // Parent of objects that will be baked.
        private Transform _container;

        // Path to save Meshes.
        private string _meshesPath;

        // Path to save Prefabs.
        private string _prefabsPath;

        [MenuItem("WhyAmLocked/Window/Lightmapped Objects Backer")]
        private static void ShowWindow()
        {
            GetWindow<LightmappedPrefabsBaker>("Lightmapped Objects Baker");
        }

        private void OnEnable()
        {
            _settings = new Settings();
        }

        private void OnGUI()
        {
            // Settings.
            GUILayout.Label("Settings");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Lightmapped material");
            _settings.LightmappedMaterial = EditorGUILayout.ObjectField(_settings.LightmappedMaterial, typeof(Material), false) as Material;
            GUILayout.EndHorizontal();
            GUILayout.Space(30);


            // Container.
            GUILayout.BeginHorizontal();
            GUILayout.Label("Container");
            _container = EditorGUILayout.ObjectField(_container, typeof(Transform), true) as Transform;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);


            // Meshes Path.
            GUILayout.BeginHorizontal();
            GUILayout.Label("Meshes path");
            _meshesPath = EditorGUILayout.TextField(_meshesPath);
            if(GUILayout.Button("Select path"))
            {
                _meshesPath = EditorUtility.OpenFolderPanel("Folder to save meshes", _meshesPath, "");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);


            // Prefabs path.
            GUILayout.BeginHorizontal();
            GUILayout.Label("Prefabs path");
            _prefabsPath = EditorGUILayout.TextField(_prefabsPath);
            if (GUILayout.Button("Select path"))
            {
                _prefabsPath = EditorUtility.OpenFolderPanel("Folder to save prefabs", _prefabsPath, "");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(30);


            // Start button.
            if (GUILayout.Button("Bake"))
            {
                BakeScene();
            }
        }

        public void BakeScene()
        {
            if (_container == null)
            {
                Debug.LogError("Container is null.");
                return;
            }

            if (_settings == null)
            {
                Debug.LogError("LightmappingSceneSettings not founded.");
                return;
            }

            if(!Directory.Exists(_meshesPath) || !Directory.Exists(_prefabsPath))
            {
                Debug.LogError("Meshes or Prefabs path is not exists.");
                return;
            }

            // Bake each object.
            foreach (Transform t in _container)
            {
                Bake(t);
            }
        }

        private void Bake(Transform obj)
        {
            // Copy object with components.
            GameObject target = Utilities.CopyObjectHierarchy(obj);

            MeshFilter[] sourceFilters = obj.GetComponentsInChildren<MeshFilter>();
            MeshFilter[] targetFilters = target.GetComponentsInChildren<MeshFilter>();
            
            // Bake each mesh in this object.
            for (int i = 0; i < targetFilters.Length; i++)
            {
                BakeMesh(sourceFilters[i], targetFilters[i], obj.name);
            }


            // Set position to zero.
            target.transform.position = Vector3.zero;

            // Save prefab as asset.
            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(target,
                 $"{Utilities.FullToProjectPath(_prefabsPath)}/{target.name}(Lightmapped).prefab", InteractionMode.AutomatedAction);


            // Destroy prefab instance from scene.
            DestroyImmediate(target);
        }

        private void BakeMesh(MeshFilter sourceFilter, MeshFilter targetFilter, string name)
        {
            // Create new mesh instance.
            Mesh m = targetFilter.sharedMesh;
            Mesh generated = Instantiate(m);
            generated.name = m.name + "(Lightmapped)";

            // Copy lightmap uvs.
            Vector4 lighmapOffset = sourceFilter.GetComponent<MeshRenderer>().lightmapScaleOffset;
            Vector2[] uvs = generated.uv2;

            // Accept lightmap scale and offset.
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(uvs[i].x * lighmapOffset.x + lighmapOffset.z,
                    uvs[i].y * lighmapOffset.y + lighmapOffset.w);
            }

            // Set Lightmap uvs to new mesh.
            generated.uv2 = uvs;

            // Create new mesh asset.
            AssetDatabase.CreateAsset(generated, $"{Utilities.FullToProjectPath(_meshesPath)}/{name}_{generated.name}.mesh");


            // Set new mesh to filter.
            targetFilter.sharedMesh = generated;

            // Set lightmapped material to renderer.
            targetFilter.GetComponent<MeshRenderer>().sharedMaterial = _settings.LightmappedMaterial;
        }
    }
}