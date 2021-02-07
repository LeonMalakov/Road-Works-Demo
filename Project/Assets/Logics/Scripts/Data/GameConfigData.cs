using UnityEngine;

namespace Combine
{
    [CreateAssetMenu(menuName = "Data/GameConfigData", fileName = "GameConfigData")]
    public class GameConfigData : ScriptableObject
    {
        [SerializeField] private Cell.Settings _cellSettings;

        [SerializeField] private Pawn.Settings _pawnSettings;

        [SerializeField] private PlayerCharacter.Settings _playerCharacterSettings;

        [SerializeField] private Pursuer.Settings _pursuerSettings;

        [SerializeField] private Grid.Settings _gridSettings;

        [SerializeField] private Road.Settings _roadSettings;


        #region Accessors
        public Cell.Settings CellSettings => _cellSettings;

        public Pawn.Settings PawnSettings => _pawnSettings;

        public PlayerCharacter.Settings PlayerCharacterSettings => _playerCharacterSettings;

        public Pursuer.Settings PursuerSettings => _pursuerSettings;

        public Grid.Settings GridSettings => _gridSettings;

        public Road.Settings RoadSettings => _roadSettings;
        #endregion
    }
}