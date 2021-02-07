using UnityEngine;

namespace Combine
{
    [RequireComponent(typeof(Pursuer))]
    public class PursuerWheelsPresenter : MonoBehaviour
    {
        [SerializeField] private Transform[] _wheels;

        #region CachedReferences
        private Pursuer _pursuer;
        #endregion


        private void Awake()
        {
            // Required component.
            _pursuer = GetComponent<Pursuer>();
        }

        private void OnEnable()
        {
            _pursuer.PositionRowChanged += OnPositionRowChanged;
        }

        private void OnDisable()
        {
            _pursuer.PositionRowChanged -= OnPositionRowChanged;
        }


        private void OnPositionRowChanged(float row)
        {
            float speed = _pursuer.Speed;

            foreach (Transform wheel in _wheels)
            {
                wheel.Rotate(-speed, 0, 0);
            }
        }
    }
}