using UnityEngine;

namespace Combine
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Target Player-controlled character.")]
        [SerializeField] private PlayerCharacter _target;
      

        private void HandleInput()
        {           
#if UNITY_EDITOR           
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SwapRight();
            } 
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SwapLeft();
            }
#endif
        }

        #region Inputs
        private void SwapRight()
        {
            _target.MoveRight();
        }

        private void SwapLeft()
        {
            _target.MoveLeft();
        }
        #endregion

        private void Update()
        {
            HandleInput();
        }
    }
}