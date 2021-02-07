using UnityEngine;

namespace Combine
{
    public class CellCollisionHandler : MonoBehaviour
    {
        [SerializeField] private Cell _cell;

        private void OnTriggerEnter(Collider other)
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                _cell.PlayerCollisionEnter(player);
                return;
            }

            Pursuer pursuer = other.GetComponent<Pursuer>();

            if (pursuer != null)
            {
                _cell.PursuerCollisionEnter(pursuer);
                return;
            }
        }
    }
}