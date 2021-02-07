using UnityEngine;

namespace Combine
{
    [RequireComponent(typeof(Game))]
    public class GameEventsHandler : MonoBehaviour
    {
        #region CachedReferences
        private Game _game;
        #endregion


        private void Awake()
        {
            // Required component.
            _game = GetComponent<Game>();
        }

        private void Start()
        {
            _game.SetState(Game.State.InGame);
        }

        private void OnEnable()
        {
            _game.PlayerCharacter.CharacterDied += OnPlayerCharacterDied;
            _game.PlayerCharacter.PositionRowChanged += OnPlayerCharacterPositionRowChanged;
        }

        private void OnDisable()
        {
            _game.PlayerCharacter.CharacterDied -= OnPlayerCharacterDied;
            _game.PlayerCharacter.PositionRowChanged -= OnPlayerCharacterPositionRowChanged;
        }


        private void OnPlayerCharacterDied(CollisionSide variant)
        {
            _game.SetState(Game.State.GameOver);
        }

        private void OnPlayerCharacterPositionRowChanged(float row)
        {
            _game.SetScore(Mathf.FloorToInt(row));
        }
    }
}