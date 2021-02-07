using UnityEngine;
using WAL.Core;

namespace Combine
{
    [RequireComponent(typeof(Grid))]
    public class GridMover : MonoBehaviour
    {
        [SerializeField] private Pawn _target;

        #region CachedReferences
        private GameConfigData _gameConfig;
        private Grid _grid;
        #endregion


        private void Awake()
        {
            // Get GameConfig reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Required component.
            _grid = GetComponent<Grid>();
        }

        private void OnEnable()
        {
            _target.PositionRowChanged += OnTargetPositionRowChanged;
        }

        private void OnDisable()
        {
            _target.PositionRowChanged -= OnTargetPositionRowChanged;
        }


        private void OnTargetPositionRowChanged(float position)
        {
            _grid.CheckAndReplaceLeadRow(position);
        }
    }
}