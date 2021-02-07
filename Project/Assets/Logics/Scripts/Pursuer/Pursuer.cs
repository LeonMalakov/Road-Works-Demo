using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combine
{
    /// <summary>
    /// Entity, that follows player.
    /// </summary>
    public class Pursuer : Pawn
    {
        [Serializable]
        public new class Settings
        {
            /// <summary>
            /// Start distance between Pursuer and PlayerCharacter.
            /// </summary>
            [Header("Start distance between Pursuer and PlayerCharacter.")]
            public float StartDistance;

            /// <summary>
            /// Maximum distance.
            /// </summary>
            [Header("Maximum distance.")]
            public float MaxDistance;

            /// <summary>
            /// Maximum speed.
            /// </summary>
            [Header("Maximum speed.")]
            public float MaxSpeed;

            /// <summary>
            /// Acceleration value using for reducing distance to PlayerCharacter.
            /// </summary>
            [Header("Acceleration value using for reducing distance to PlayerCharacter.")]
            public float AccelerationNormal;

            /// <summary>
            /// Acceleration value using for increasing relative speed to 0..
            /// </summary>
            [Header("Acceleration value using for increasing relative speed to 0.")]
            public float AccelerationToZero;

            /// <summary>
            /// The value that is subtracted from the speed when the pursuer hits an obstacle.
            /// </summary>
            [Header("The value that is subtracted from the speed when the pursuer hits an obstacle.")]
            public float HitSpeedPenalty;

            /// <summary>
            /// Distance between Pursuer and PlayerCharacter when Character dies.
            /// </summary>
            [Header("Distance between Pursuer and PlayerCharacter when Character dies.")]
            public float KillDistance;

            /// <summary>
            /// Speed after player character died.
            /// </summary>
            [Header("Speed after player character died.")]
            public float PostCharacterDeadSpeed;
        }

        [SerializeField] private PlayerCharacter _playerCharacter;

        [SerializeField] private Grid _grid;


        private Queue<GridCoordinates> _nodes = new Queue<GridCoordinates>();


        /// <summary>
        /// Distance between Pursuer and PlayerCharacter.
        /// </summary>
        public float Distance { get; private set; }

        /// <summary>
        /// Additional speed (Relative to Player).
        /// </summary>
        public float AdditionalSpeed { get; private set; }

        public bool IsStopped { get; private set; }

        public float Speed => AdditionalSpeed + _playerCharacter.Speed;


        protected override void Awake()
        {
            base.Awake();

            // Set start distance.
            Distance = _gameConfig.PursuerSettings.StartDistance;
        }

        private void OnEnable()
        {
            _playerCharacter.PositionLineChanged += OnPlayerCharacterPositionLineChanged;
            _playerCharacter.CharacterDied += OnPlayerCharacterDied;
        }
      

        private void OnDisable()
        {
            _playerCharacter.PositionLineChanged -= OnPlayerCharacterPositionLineChanged;
            _playerCharacter.CharacterDied -= OnPlayerCharacterDied;
        }

        private void Update()
        {
            if (!IsStopped)
            {
                Move();
                IncreaseSpeed();
            }
        }


        public void HitObstacle()
        {
            AdditionalSpeed -= _gameConfig.PursuerSettings.HitSpeedPenalty;
        }

        public void HitNotWalkable()
        {
            if(!_playerCharacter.IsAlive)
                IsStopped = true;
        }


        private void Move()
        {
            if (_playerCharacter.IsAlive)
            {
                // Kill player if distance less than killDistance.
                if (Distance <= _gameConfig.PursuerSettings.KillDistance)
                {
                    _playerCharacter.Die();
                }

                // Check line change clauses.
                if (_nodes.Count > 0)
                {
                    GridCoordinates node = _nodes.Peek();

                    // Change position in line.
                    if (Position.Row >= node.Row)
                    {
                        _nodes.Dequeue();

                        MoveToLine(node.Line);
                    }
                }
            }

            // Reduce distance with speed.
            Distance -= AdditionalSpeed * Time.deltaTime;

            if (Distance > _gameConfig.PursuerSettings.MaxDistance)
                Distance = _gameConfig.PursuerSettings.MaxDistance;


            // Set position row(Z) relative to player.
            SetPositionRow(_playerCharacter.Position.Row - Distance); 
        }

        private void IncreaseSpeed()
        {
            if (AdditionalSpeed < 0)
            {
                AdditionalSpeed += _gameConfig.PursuerSettings.AccelerationToZero * _playerCharacter.Speed * Time.deltaTime;
            }
            else if (AdditionalSpeed < _gameConfig.PursuerSettings.MaxSpeed)
            {
                AdditionalSpeed += _gameConfig.PursuerSettings.AccelerationNormal * _playerCharacter.Speed * Time.deltaTime;
            }
        }


        private void OnPlayerCharacterPositionLineChanged(int position)
        {
            // Add line change node.
            _nodes.Enqueue(new GridCoordinates(position, _playerCharacter.Position.Row));
        }

        private void OnPlayerCharacterDied(CollisionSide variant)
        {
            AdditionalSpeed = _gameConfig.PursuerSettings.PostCharacterDeadSpeed;
        }
    }
}