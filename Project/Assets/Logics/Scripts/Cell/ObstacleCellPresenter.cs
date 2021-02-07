using UnityEngine;

namespace Combine
{
    public class ObstacleCellPresenter : MonoBehaviour
    {
        [SerializeField] private ObstacleCell _cell;

        [SerializeField] private GameObject _sourceObject;
        [SerializeField] private GameObject _normalObject;
        [SerializeField] private GameObject _destroyedObject;


        private void OnEnable()
        {
            // Update state.
            UpdateObjectsActiveStates();

            // Subscribe.
            _cell.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            // Unsubscribe.
            _cell.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            UpdateObjectsActiveStates();
        }

        private void UpdateObjectsActiveStates()
        {
            _sourceObject.SetActive(false);
            _normalObject.SetActive(false);
            _destroyedObject.SetActive(false);

            switch (_cell.CurrentState)
            {
                case ObstacleCell.State.Source:
                    _sourceObject.SetActive(true);
                    break;

                case ObstacleCell.State.Normal:
                    _normalObject.SetActive(true);
                    break;

                case ObstacleCell.State.Destroyed:
                    _destroyedObject.SetActive(true);
                    break;
            }
        }
    }
}