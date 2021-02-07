using System.Collections;
using UnityEngine;
using WAL.Core;

namespace Combine
{
    [RequireComponent(typeof(Pawn))]
    public class PawnPresenter : MonoBehaviour
    {
        #region CachedReferences
        private GameConfigData _gameConfig;
        private RelativeCoordinates _relativeCoordinates;
        private Transform _transform;
        private Pawn _pawn;
        #endregion

        private Coroutine _moveCoroutine;


        private void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Get RelativeCoordinates reference via container.
            _relativeCoordinates = Globals.Instance.GetDependency<RelativeCoordinates>();

            // Required component.
            _pawn = GetComponent<Pawn>();

            // Get Transform reference.
            _transform = transform;
        }

        private void OnEnable()
        {
            ApplyPosition();

            // Subscribe. 
            _pawn.PositionLineChanged += OnPositionLineChanged;
            _pawn.PositionRowChanged += OnPositionRowChanged;
            _pawn.FreezeStateChanged += OnFreezeStateChanged;
        }

        private void OnDisable()
        {
            // Unsubscribe. 
            _pawn.PositionLineChanged -= OnPositionLineChanged;
            _pawn.PositionRowChanged -= OnPositionRowChanged;
            _pawn.FreezeStateChanged -= OnFreezeStateChanged;
        }


        private void OnPositionLineChanged(int position)
        {
            MoveToPosition(position);
        }

        private void OnPositionRowChanged(float position)
        {
            SetToPositionRow(position);
        }

        private void OnFreezeStateChanged(bool state)
        {
            if (state == true)
                StopCurrentTranslation();
            else
                ApplyPosition();
        }


        // Instant Row(Z) position applying.
        private void SetToPositionRow(float pos)
        {
            Vector3 target = _relativeCoordinates.ToWorldPosition(new Vector3(_transform.position.x, _transform.position.y, pos));

            _transform.position = target;
        }

        // Perform movement to position. Starts translation coroutine, if not instant. Else apply position instantly.
        private void MoveToPosition(int pos, bool instant = false)
        {
            StopCurrentTranslation();

            // End(Target) position.
            float targetPosX = pos * _gameConfig.CellSettings.CellSize;

            // Start translation coroutine, or apply position instantly.
            if (!instant)
                _moveCoroutine = StartCoroutine(TranslationCoroutine(_transform.position.x, targetPosX));
            else
                _transform.position = new Vector3(targetPosX, _transform.position.y, _transform.position.z);
        }

        // Coroutine translation from startPos to endPos.
        private IEnumerator TranslationCoroutine(float startPosX, float endPosX)
        {
            // Speed by distance multipler. 
            // 1: if it's full movement from one cell to another. 
            // From 0 to 1: if it's part of movement.
            float speedByDistMultipler = Mathf.Abs(endPosX - startPosX) / _gameConfig.CellSettings.CellSize;

            // Current (lerped) position.
            float currentPosX;

            float t = 0;

            while (t < 1)
            {
                // Increase time with DeltaTime, speedMultipler and speed.
                t += Time.deltaTime * speedByDistMultipler * _gameConfig.PawnSettings.MoveSpeed;

                // Lerp current position.
                currentPosX = Mathf.Lerp(startPosX, endPosX, _gameConfig.PawnSettings.MoveCurve.Evaluate(t));

                // Apply current position.
                _transform.position = new Vector3(currentPosX, _transform.position.y, _transform.position.z);

                yield return null;
            }

            // Apply end (target) position.
            _transform.position = new Vector3(endPosX, _transform.position.y, _transform.position.z);

            // Free coroutine.
            _moveCoroutine = null;
        }


        private void ApplyPosition()
        {
            // Apply position.
            MoveToPosition(_pawn.Position.Line, true);
            SetToPositionRow(_pawn.Position.Row);
        }

        private void StopCurrentTranslation()
        {
            // Stop current translation.
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
        }
    }
}