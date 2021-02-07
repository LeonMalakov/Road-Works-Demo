using UnityEngine;
using WAL.Core;

namespace Combine
{
    [CreateAssetMenu(fileName = "CellsData", menuName = "Data/CellsData")]
    public class CellsData : ScriptableObject
    {
        /// <summary>
        /// Cells prefabs library.
        /// </summary>
        [Header("Cells prefabs library.")]
        public EnumMatching<BiomeType, EnumMatching<CellType, Wrapper<GameObject[]>>> CellsPrefabs;

    }
}