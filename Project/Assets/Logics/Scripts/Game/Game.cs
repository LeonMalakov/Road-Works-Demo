using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combine
{
    public class Game : MonoBehaviour
    {
        public enum State
        {
            InGame,
            GameOver
        }


        [SerializeField] private PlayerCharacter _playerCharacter;

        [SerializeField] private Grid _grid;

        [SerializeField] private Road _road;


        public PlayerCharacter PlayerCharacter => _playerCharacter;

        public State CurrentState { get; private set; }

        public int Score { get; private set; }


        public event Action<State> StateChanged;

        public event Action<int> ScoreChanged;


        public void SetState(State state)
        {
            CurrentState = state;

            StateChanged?.Invoke(state);
        }

        public void SetScore(int score)
        {
            Score = score;
            ScoreChanged?.Invoke(score);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}