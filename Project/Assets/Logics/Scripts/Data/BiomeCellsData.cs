using System;
using UnityEngine;

namespace Combine
{
    [CreateAssetMenu(fileName = "BiomeCellsData", menuName = "Data/BiomeCellsData")]
    public class BiomeCellsData : ScriptableObject
    {
        [Serializable]
        public class ObstacleCell
        {
            public GameObject Source;
            public GameObject Normal;
            public GameObject Destroyed;
        }

        public GameObject[] EmptyCells;

        public GameObject[] NotWalkableCells;

        public ObstacleCell[] ObstacleCells;

        public GameObject[] BorderCells;

    }
}