using System;

namespace Combine
{
    public class ObstacleCell : Cell
    {
        // Obstacle cell state.
        public enum State
        {
            Source,
            Normal,
            Destroyed
        }

        public override CellType Type => CellType.Obstacle;

        public State CurrentState { get; private set; }

        public event Action StateChanged;


        public override void PlayerCollisionEnter(PlayerCharacter player)
        {
            if (CurrentState == State.Source)
                SetState(State.Normal);
        }

        public override void PursuerCollisionEnter(Pursuer pursuer)
        {
            if (CurrentState == State.Normal)
            {
                SetState(State.Destroyed);
                pursuer.HitObstacle();
            }
        }

        protected override void OnResetObject()
        {
            SetState(State.Source);
        }

        private void SetState(State state)
        {
            CurrentState = state;

            StateChanged?.Invoke();
        }
    }
}