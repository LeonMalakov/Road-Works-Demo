using System.Collections.Generic;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    public class CellsPool : MonoBehaviour
    {
        #region CachedReferences
        private CellsData _cellsData;
        #endregion

        // Anchor for disabled cells.
        private Transform _poolParent;

        // Collection of pooled cells.
        private class PooledCellsCollection
        {
            private Stack<Cell>[,] _cells;

            public PooledCellsCollection()
            {
                // Initialize collection.
                _cells = new Stack<Cell>[(int)BiomeType.Total, (int)CellType.Total];
                for (int i = 0; i < _cells.GetLength(0); i++)
                    for (int j = 0; j < _cells.GetLength(1); j++)
                        _cells[i, j] = new Stack<Cell>();
            }

            // Returns cell or NULL.
            public Cell Pop(CellType cell, BiomeType biome)
            {
                int cellInd = (int)cell;
                int biomeInd = (int)biome;

                if (_cells[biomeInd, cellInd].Count > 0)
                    return _cells[biomeInd, cellInd].Pop();

                return null;
            }

            // Pushes cell into collection.
            public void Push(Cell cell)
            {
                _cells[(int)cell.Biome, (int)cell.Type].Push(cell);
            }
        }

        private PooledCellsCollection _cells;

        private void Awake()
        {
            // Get CellsData reference via container.
            _cellsData = Globals.Instance.GetData<CellsData>();

            // Get anchor reference and disable.
            _poolParent = new GameObject("Cells").transform;
            _poolParent.SetParent(transform);
            _poolParent.gameObject.SetActive(false);

            // Cells collection initialization.
            _cells = new PooledCellsCollection();
        }

        /// <summary>
        /// Returns Cell instance from pool. If is not exists, instantiate new.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cell Spawn(CellType id, BiomeType biome)
        {
            Cell cell;

            // Pop Cell if exist.

            cell = _cells.Pop(id, biome);

            // Instantiate cell if not exitst.
            if (cell == null)
            {
                cell = CreateCellInstance(id, biome);
            }

            // Reset cell and clear it's parent.
            cell.ResetObject();
            cell.transform.SetParent(null);

            return cell;
        }

        /// <summary>
        /// Deactivates Cell and pushes into pool.
        /// </summary>
        /// <param name="cell"></param>
        public void Despawn(Cell cell)
        {
            // Set anchor as cell's parent: anchor is disabled therefore cell will disable too.
            cell.transform.SetParent(_poolParent);

            // Push in pooled collection.
            _cells.Push(cell);
        }

        // Create a new RANDOM Cell based on prefab.
        private Cell CreateCellInstance(CellType type, BiomeType biome)
        {
            // Get prefabs array with CellType and BiomeType.
            GameObject[] cellsPrefabs = _cellsData.CellsPrefabs.Get(biome).Get(type).Item;

            // Get RANDOM cell prefab from array.
            int ind = UnityEngine.Random.Range(0, cellsPrefabs.Length);
            GameObject cellPrefab = cellsPrefabs[ind];

            // Instantiate cell. 
            Cell cell = Instantiate(cellPrefab).GetComponent<Cell>();

            return cell;
        }
    }
}