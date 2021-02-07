using System;
using System.Collections.Generic;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    public class Road : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            /// <summary>
            /// Offset from player character position.
            /// </summary>
            [Header("Offset from player character position.")]
            public float Offset;

            /// <summary>
            /// Offset from line changed position to position new segment will be created.
            /// </summary>
            [Header("Offset from line changed position to position new segment will be created.")]
            public float LineChangeOffset;
        }

        [SerializeField] private PlayerCharacter _playerCharacter;

        [SerializeField] private Grid _grid;

        [SerializeField] private RoadPool _pool;


        #region CachedReferences
        private GameConfigData _gameConfig;
        private Transform _transform;
        #endregion

        private RoadSegment _activeSegment;
        private bool _isSubscribedForActiveSegment;
        private bool _waitingForNewSegment;
        private float _lastLineChangedRow;

        public Queue<RoadSegment> RoadSegments { get; private set; } = new Queue<RoadSegment>();


        private void Awake()
        {
            // Get GameConfig reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            _transform = transform;
        }

        private void OnEnable()
        {
            CreateNewSegment();

            _playerCharacter.PositionLineChanged += OnPlayerCharacterPositionLineChanged;
            _playerCharacter.PositionRowChanged += OnPlayerCharacterPositionRowChanged;

            if (_activeSegment != null && !_isSubscribedForActiveSegment)
            {
                _activeSegment.BuildingComplited += OnActiveSegmentBuildingComplited;
                _isSubscribedForActiveSegment = true;
            }
        }

        private void OnDisable()
        {
            _playerCharacter.PositionLineChanged -= OnPlayerCharacterPositionLineChanged;
            _playerCharacter.PositionRowChanged -= OnPlayerCharacterPositionRowChanged;

            UnsubscribeLastSegment();
        }


        private void UnsubscribeLastSegment()
        {
            // Unsubscribe from last active segment if it exists.
            if (_activeSegment != null && _isSubscribedForActiveSegment)
            {
                _activeSegment.BuildingComplited -= OnActiveSegmentBuildingComplited;
                _isSubscribedForActiveSegment = false;
            }
        }

        private void CreateNewSegment(bool relativeToLast = false)
        {
            // Spawn new segment.
            GridCoordinates pos = new GridCoordinates(_playerCharacter.Position.Line, _playerCharacter.Position.Row + _gameConfig.RoadSettings.Offset);
            if (relativeToLast && _activeSegment != null)
                pos.Row = _activeSegment.Position.Row + _gameConfig.CellSettings.CellSize;

            _activeSegment = _pool.Spawn(_transform);
            _activeSegment.Setup(pos);
            RoadSegments.Enqueue(_activeSegment);

            // Subscribe for active segment.
            _activeSegment.BuildingComplited += OnActiveSegmentBuildingComplited;
            _isSubscribedForActiveSegment = true;
        }

        private void RemoveOldestSegment()
        {
            if (RoadSegments.Count > 0)
            {
                RoadSegment segment = RoadSegments.Dequeue();
                _pool.Despawn(segment);
            }
        }


        private void OnPlayerCharacterPositionLineChanged(int line)
        {
            _lastLineChangedRow = _playerCharacter.Position.Row;
            _waitingForNewSegment = true;

            UnsubscribeLastSegment();
        }

        private void OnPlayerCharacterPositionRowChanged(float row)
        {
            // Create new segment after offset when line changed.
            if (_waitingForNewSegment && row > _lastLineChangedRow + _gameConfig.RoadSettings.LineChangeOffset)
            {
                CreateNewSegment();
                _waitingForNewSegment = false;
            }

            // Increase active segment length.
            if (_activeSegment != null)
            {
                // Build active road segment.
                _activeSegment.IncreaseLength(row + _gameConfig.RoadSettings.Offset);
            }

            // Segment out of playable space check.
            if (RoadSegments.Count > 0)
            {
                RoadSegment segment = RoadSegments.Peek();

                if (segment.Position.Row < _gameConfig.GridSettings.StartPosition + _gameConfig.CellSettings.CellSize * (_grid.RowsOffset - 1))
                {
                    RemoveOldestSegment();
                }
            }
        }

        private void OnActiveSegmentBuildingComplited()
        {
            UnsubscribeLastSegment();
            CreateNewSegment(true);
        }
    }
}