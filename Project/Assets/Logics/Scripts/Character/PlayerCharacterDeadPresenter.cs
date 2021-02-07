using UnityEngine;
using WAL.Core;

namespace Combine
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerCharacterDeadPresenter : MonoBehaviour
    {
        #region CachedReferences
        private GameConfigData _gameConfig;
        private PlayerCharacter _playerCharacter;
        #endregion


        private void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Required component.
            _playerCharacter = GetComponent<PlayerCharacter>();
        }

        private void OnEnable()
        {
            _playerCharacter.CharacterDied += OnCharacterDied;
        }

        private void OnDisable()
        {
            _playerCharacter.CharacterDied -= OnCharacterDied;
        }


        private void OnCharacterDied(CollisionSide variant)
        {
            Debug.Log($"Dead variant: {variant}");
        }
    }
}