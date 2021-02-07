using System.Collections;
using UnityEngine;

namespace Combine
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerCharacterAnimator : MonoBehaviour
    {
        private const string AnimatorSpeed = "Speed";

        private const string AnimatorIsDead = "IsDead";

        private const string AnimatorDieVariant = "DieVariant";

        [SerializeField] private Animator _animator;

        [SerializeField] private Transform _rigHead;

        [SerializeField] private Transform _rigRHand;

        [SerializeField] private Transform _helmet;

        [SerializeField] private Transform _shovel;

        [SerializeField] private ParticleSystem _digEffect;

        [SerializeField] private ParticleSystem _buildEffect;


        private bool _isEquipmentsDropped;


        #region CachedReferences
        private PlayerCharacter _playerCharacter;
        #endregion


        private void Awake()
        {
            // Required component.
            _playerCharacter = GetComponent<PlayerCharacter>();
        }

        private void Start()
        {
            _helmet.SetParent(_rigHead);
            _shovel.SetParent(_rigRHand);
        }

        private void OnEnable()
        {
            _playerCharacter.SpeedChanged += OnSpeedChanged;
            _playerCharacter.CharacterDied += OnCharacterDied;
        }

        private void OnDisable()
        {
            _playerCharacter.SpeedChanged -= OnSpeedChanged;
            _playerCharacter.CharacterDied -= OnCharacterDied;
        }


        private IEnumerator DropEquipments(float forceMultipler)
        {
            Vector3 shovelStartPos = _shovel.position;
            Vector3 helmetStartPos = _helmet.position;

            yield return new WaitForFixedUpdate();

            _shovel.SetParent(null);
            _helmet.SetParent(null);

            Rigidbody shovelRb = _shovel.GetComponent<Rigidbody>();
            Rigidbody helmetRb = _helmet.GetComponent<Rigidbody>();

            shovelRb.isKinematic = false;
            helmetRb.isKinematic = false;

            shovelRb.AddForce((_shovel.position - shovelStartPos) * forceMultipler, ForceMode.Impulse);
            helmetRb.AddForce((_helmet.position - helmetStartPos) * forceMultipler, ForceMode.Impulse);
        }


        private void OnSpeedChanged(float speed)
        {
            _animator.SetFloat(AnimatorSpeed, speed);
        }

        private void OnCharacterDied(CollisionSide variant)
        {
            _buildEffect.Stop();
            _animator.SetBool(AnimatorIsDead, true);
            _animator.SetInteger(AnimatorDieVariant, (int)variant);
        }


        private void AnimEvent_ShovelInUpperPosition()
        {
            _digEffect.Play();
        }

        private void AnimEvent_DropEquipments(float forceMultipler)
        {
            if (!_isEquipmentsDropped)
            {
                _isEquipmentsDropped = true;
                StartCoroutine(DropEquipments(forceMultipler));
            }
        }
    }
}