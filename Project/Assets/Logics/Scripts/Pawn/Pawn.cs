using System;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    public class Pawn : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            /// <summary>
            /// Curve using for line to line movement.
            /// </summary>
            [Header("Curve using for line to line movement.")]
            public AnimationCurve MoveCurve;

            /// <summary>
            /// Line to line move speed.
            /// </summary>
            [Header("Line to line move speed.")]
            public float MoveSpeed;
        }

        #region CachedReferences
        protected GameConfigData _gameConfig;
        #endregion


        public GridCoordinates Position { get; private set; }

        public bool IsFreezed { get; private set; }


        public event Action<int> PositionLineChanged;

        public event Action<float> PositionRowChanged;

        public event Action<bool> FreezeStateChanged;


        protected virtual void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();
        }


        protected void SetPositionRow(float pos)
        {
            if (IsFreezed) return;

            Position = new GridCoordinates(Position.Line, pos);
            PositionRowChanged?.Invoke(pos);
        }

        protected void MoveToLine(int line)
        {
            if (IsFreezed) return;

            if (line < -_gameConfig.GridSettings.PlayableLinesCount / 2 || line > _gameConfig.GridSettings.PlayableLinesCount / 2)
            {
                throw new ArgumentException("Line is out of playable lines range.");
            }

            Position = new GridCoordinates(line, Position.Row);
            PositionLineChanged?.Invoke(Position.Line);
        }

        protected void SetFreezeState(bool state)
        {
            IsFreezed = state;
            FreezeStateChanged?.Invoke(IsFreezed);
        }
    }
}