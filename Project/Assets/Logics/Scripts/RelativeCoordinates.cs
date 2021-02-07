using System;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    /// <summary>
    /// Relative position to Row(Z) of origin.
    /// </summary>
    public class RelativeCoordinates : MonoBehaviour
    {
        [SerializeField] private Pawn _origin;

        #region CachedReferences
        private GameConfigData _gameConfig;
        #endregion


        public event Action<float> OriginPositionRowChanged;


        private void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();
        }

        private void OnEnable()
        {
            _origin.PositionRowChanged += OnOriginPositionRowChanged;
        }

        private void OnDisable()
        {
            _origin.PositionRowChanged -= OnOriginPositionRowChanged;
        }


        public Vector3 ToWorldPosition(GridCoordinates pos)
        {
            return new Vector3(pos.Line * _gameConfig.CellSettings.CellSize, 0, pos.Row - _origin.Position.Row);
        }

        public Vector3 ToWorldPosition(Vector3 pos)
        {
            return new Vector3(pos.x, pos.y, pos.z - _origin.Position.Row);
        }


        private void OnOriginPositionRowChanged(float row)
        {
            OriginPositionRowChanged?.Invoke(row);
        }
    }
}