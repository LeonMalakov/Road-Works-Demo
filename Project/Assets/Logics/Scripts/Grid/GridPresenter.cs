using UnityEngine;
using WAL.Core;

namespace Combine
{
    [RequireComponent(typeof(Grid))]
    public class GridPresenter : MonoBehaviour
    {
        [SerializeField] private CellsPool _pool;

        #region CachedReferences     
        private GameConfigData _gameConfig;
        private RelativeCoordinates _relativeCoordinates;
        private Transform _transform;
        private Grid _grid;
        #endregion

        private Transform[] _rows;


        private void Awake()
        {
            // Get GameConfig reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Get RelativeCoordinates reference via container.
            _relativeCoordinates = Globals.Instance.GetDependency<RelativeCoordinates>();

            _transform = transform;

            // Required component.
            _grid = GetComponent<Grid>();

            InitializeRows();
        }

        private void OnEnable()
        {
        //    ApplyCellsPositions(_grid.LeadRow);

        //    _grid.LeadRowChanged += OnLeadRowChanged;
            _grid.RowCellsChanged += OnRowCellsChanged;
        }

        private void OnDisable()
        {
        //    _grid.LeadRowChanged -= OnLeadRowChanged;
            _grid.RowCellsChanged -= OnRowCellsChanged;
        }

        private void Update()
        {
            ApplyCellsPositions(_grid.LeadRow);
        }


        // Creates rows GameObjects.
        private void InitializeRows()
        {
            _rows = new Transform[_gameConfig.GridSettings.RowsCount];

            for(int i = 0; i < _rows.Length; i++)
            {
               _rows[i] = new GameObject($"Row[{i}]").transform;
                _rows[i].parent = _transform;
            }
        }

        private void ApplyCellsPositions(int lead)
        {
            float startPos = _gameConfig.GridSettings.StartPosition;
            float cellSize = _gameConfig.CellSettings.CellSize;

            for (int i = 0; i < _rows.Length; i++)
            {
                float zPos;

                if (i < lead)
                    zPos = startPos + ((_rows.Length - lead + i) + _grid.RowsOffset) * cellSize;
                else
                    zPos = startPos + ((i - lead) + _grid.RowsOffset) * cellSize;

                _rows[i].localPosition = _relativeCoordinates.ToWorldPosition(new GridCoordinates(0, zPos));
            }
        }


        private void OnRowCellsChanged(int rowIndex)
        {
            // Despawn old cells.
            foreach (Cell c in _rows[rowIndex].GetComponentsInChildren<Cell>())
                _pool.Despawn(c);


            // Spawn new cells.
            float cellSize = _gameConfig.CellSettings.CellSize;
            float startX = -(_gameConfig.GridSettings.TotalLinesCount) / 2 * cellSize;

            int index = 0;
            foreach(CellType cell in _grid.GetRowCells(rowIndex))
            {
                Transform c = _pool.Spawn(cell, _grid.Biome).transform;
                c.SetParent(_rows[rowIndex]);
                c.localPosition = new Vector3(startX + index++ * cellSize, 0, 0);
            }
        }
    }
}
