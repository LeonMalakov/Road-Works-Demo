using UnityEngine;

namespace Combine
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private Game _game;

        [SerializeField] private GameOverMenu _menu;

        [SerializeField] private ScoreText _scoreText;

        [SerializeField] private Overlay _overlay;


        private bool _isRestarting;


        private void OnEnable()
        {
            _scoreText.SetScore(_game.Score);

            _game.StateChanged += OnGameStateChanged;
            _game.ScoreChanged += OnGameScoreChanged;
            _menu.RestartClicked += OnMenuRestartClicked;
        }

        private void OnDisable()
        {
            _game.StateChanged -= OnGameStateChanged;
            _game.ScoreChanged -= OnGameScoreChanged;
            _menu.RestartClicked -= OnMenuRestartClicked;
        }


        private void Restart()
        {
            if (!_isRestarting)
            {
                _isRestarting = true;
                _overlay.FadeIn(_game.Restart);
            }
        }


        private void OnGameStateChanged(Game.State state)
        {
            switch (state)
            {
                case Game.State.InGame:
                    _overlay.FadeOut();
                    break;

                case Game.State.GameOver:
                    _menu.Show();
                    break;
            }
        }

        private void OnGameScoreChanged(int score)
        {
            _scoreText.SetScore(score);
        }

        private void OnMenuRestartClicked()
        {
            Restart();
        }
    }
}