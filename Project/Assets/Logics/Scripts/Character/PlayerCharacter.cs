using System;
using UnityEngine;

namespace Combine
{
    /// <summary>
    /// Player-controlled character.
    /// </summary>
    public class PlayerCharacter : Pawn
    {
        [Serializable]
        public new class Settings
        {
            /// <summary>
            /// Speed when game started.
            /// </summary>
            [Header("Speed when game started.")]
            public float StartSpeed;

            /// <summary>
            /// Acceleration value using for increase speed.
            /// </summary>
            [Header("Acceleration value using for increase speed.")]
            public float Acceleration;
        }


        /// <summary>
        /// Is player character alive.
        /// </summary>
        public bool IsAlive { get; private set; } = true;

        /// <summary>
        /// Current speed.
        /// </summary>
        public float Speed { get; private set; }


        public event Action<float> SpeedChanged;

        public event Action<CollisionSide> CharacterDied;


        protected override void Awake()
        {
            base.Awake();

            // Apply start speed.
            Speed = _gameConfig.PlayerCharacterSettings.StartSpeed;
        }

        private void Update()
        {
            if (IsAlive && !IsFreezed)
            {
                PerformMove();
                IncreaseSpeed();
            }
        }


        /// <summary>
        /// Move to left line from current.
        /// </summary>
        /// <returns>True, if it is possible and character starts moving.<br/>
        /// False, if character is dead OR is on the extreme left line and movement isn't possible.</returns>
        public bool MoveLeft()
        {
            if (!IsAlive || IsFreezed)
                return false;

            if (Position.Line <= -_gameConfig.GridSettings.PlayableLinesCount / 2)
                return false;

            MoveToLine(Position.Line - 1);

            return true;
        }

        /// <summary>
        /// Move to right line from current.
        /// </summary>
        /// <returns>True, if it is possible and character starts moving.<br/>
        /// False, if character is dead OR is on the extreme right line and movement isn't possible.</returns>
        public bool MoveRight()
        {
            if (!IsAlive || IsFreezed)
                return false;

            if (Position.Line >= _gameConfig.GridSettings.PlayableLinesCount / 2)
                return false;

            MoveToLine(Position.Line + 1);

            return true;
        }

        public void Die(CollisionSide variant = CollisionSide.Front)
        {
            if (!IsAlive) return;

            IsAlive = false;
            Speed = 0;
            SetFreezeState(true);

            CharacterDied?.Invoke(variant);
        }


        private void PerformMove()
        {
            float translation = Speed * Time.deltaTime;

            SetPositionRow(Position.Row + translation);
        }

        private void IncreaseSpeed()
        {
            Speed = _gameConfig.PlayerCharacterSettings.StartSpeed + Mathf.Sqrt(_gameConfig.PlayerCharacterSettings.Acceleration * Time.timeSinceLevelLoad);
            SpeedChanged?.Invoke(Speed);
        }
    }
}