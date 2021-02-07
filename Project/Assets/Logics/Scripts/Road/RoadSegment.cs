using System;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    public class RoadSegment : MonoBehaviour
    {
        // Adds to segment length when it is finished to fix line between segments.
        private const float LenghtError = 0.1f;

        #region CachedReferences
        private GameConfigData _gameConfig;
        private Transform _transform;
        #endregion


        public GridCoordinates Position { get; private set; }

        public float Length { get; private set; }

        public bool IsFinished { get; private set; }


        public event Action BuildingComplited;

        public event Action<float> LengthChanged;


        private void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            _transform = transform;
        }


        public void ResetObject()
        {
            Length = 0;
            IsFinished = false;

            LengthChanged?.Invoke(Length);
        }

        public void Setup(GridCoordinates pos)
        {
            Position = pos;
        }

        public void IncreaseLength(float row)
        {
            if (!IsFinished)
            {
                Length = row - Position.Row;           

                if (Length >= _gameConfig.CellSettings.CellSize)
                {
                    Length = _gameConfig.CellSettings.CellSize + LenghtError;
                    LengthChanged?.Invoke(Length);

                    IsFinished = true;
                    BuildingComplited?.Invoke();
                }
                else
                {
                    LengthChanged?.Invoke(Length);
                }
            }
        }
    }
}