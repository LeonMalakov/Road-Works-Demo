using System;
using System.Collections.Generic;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    public class Grid : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            /// <summary>
            /// Number of lines in row.
            /// </summary>
            [Header("Number of lines in row.")]
            public int TotalLinesCount;

            /// <summary>
            /// Number of lines where player can move.
            /// </summary>
            [Header("Number of lines where player can move.")]
            public int PlayableLinesCount;

            /// <summary>
            ///  Number of rows.
            /// </summary>
            [Header("Number of rows.")]
            public int RowsCount;

            /// <summary>
            /// Z-coordinate where first row will set.
            /// </summary>
            [Header("Z-coordinate where first row will set.")]
            public float StartPosition;

            /// <summary>
            /// Number of lead lines (paths).
            /// </summary>
            [Header("Number of lead lines (paths).")]
            public int LeadLinesCount;

            /// <summary>
            /// Every *value* line, obstacle will be spawned.
            /// </summary>
            [Header("Every *value* line, obstacle will be spawned.")]
            public int ObstacleOccurrenceFrequency;
        }


        #region CachedReferences
        private GameConfigData _gameConfig;
        #endregion

        // Generator, using for generate row's cells.
        private GridGenerator _generator;


        private CellType[,] _cells;

        public int LeadRow { get; private set; }

        public BiomeType Biome { get; private set; }

        public int RowsOffset { get; private set; }


        public event Action<int> LeadRowChanged;

        public event Action<int> RowCellsChanged;


        private void Awake()
        {
            // Get GameConfig reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Create generator instance.
            _generator = new GridGenerator(_gameConfig);
        }

        private void Start()
        {
            // Select random biome.
            Biome = (BiomeType)UnityEngine.Random.Range(0, (int)BiomeType.Total);

            InitilizeCells();
        }


        /// <summary>
        /// Returns CellType-s in row.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IEnumerable<CellType> GetRowCells(int index)
        {
            for (int i = 0; i < _gameConfig.GridSettings.TotalLinesCount; i++)
                yield return _cells[index, i];
        }

        public void CheckAndReplaceLeadRow(float position)
        {
            // Set lead row to end.
            if (position >= _gameConfig.CellSettings.CellSize * (RowsOffset+1))
            {
                RowsOffset++;

                // Update row's cells.
                SelectRowCells(LeadRow);

                // Select next lead row.
                LeadRow = ((LeadRow >= _gameConfig.GridSettings.RowsCount - 1) ? 0 : (LeadRow + 1));

                LeadRowChanged?.Invoke(LeadRow);
            }
        }


        // Generates CellType-s for row using generator.
        private void SelectRowCells(int row, bool isInitial = false)
        {
            int index = 0;
            foreach (CellType cell in _generator.Next(isInitial))
            {
                _cells[row, index++] = cell;
            }

            RowCellsChanged?.Invoke(row);
        }

        // Generates all row's CellTypes.
        private void InitilizeCells()
        {
            _cells = new CellType[_gameConfig.GridSettings.RowsCount, _gameConfig.GridSettings.TotalLinesCount];

            for (int i = 0; i < _gameConfig.GridSettings.RowsCount; i++)
            {
                SelectRowCells(i, true);
            }
        }


    }
}