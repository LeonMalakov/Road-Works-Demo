using UnityEngine;
using WAL.Core;

namespace Combine
{
    [RequireComponent(typeof(RoadSegment))]
    public class RoadSegmentPresenter : MonoBehaviour
    {
        #region CachedReferences
        private GameConfigData _gameConfig;
        private RelativeCoordinates _relativeCoordinates;
        private Transform _transform;
        private MeshRenderer _renderer;
        private RoadSegment _roadSegment;
        #endregion

        private Material _material;


        private void Awake()
        {
            // Get GameConfigData reference via container.
            _gameConfig = Globals.Instance.GetData<GameConfigData>();

            // Get RelativeCoordinates reference via container.
            _relativeCoordinates = Globals.Instance.GetDependency<RelativeCoordinates>();

            // Required component.
            _roadSegment = GetComponent<RoadSegment>();

            // Get Transform reference.
            _transform = transform;

            _renderer = GetComponentInChildren<MeshRenderer>();

            // Clone material.
            Material originMaterial = _renderer.sharedMaterial;
            _material = Instantiate(originMaterial);
            _renderer.sharedMaterial = _material;

            _material.SetFloat("_PositionZ", _transform.position.z);
            _material.SetFloat("_Progress", 0);
        }

        private void OnEnable()
        {
            ApplyPosition();

            _relativeCoordinates.OriginPositionRowChanged += OnOriginPositionRowChanged;
            _roadSegment.LengthChanged += OnRoadSegmentLengthChanged;
        }

        private void OnDisable()
        {
            _relativeCoordinates.OriginPositionRowChanged -= OnOriginPositionRowChanged;
            _roadSegment.LengthChanged -= OnRoadSegmentLengthChanged;
        }


        private void ApplyPosition()
        {
            _transform.position = _relativeCoordinates.ToWorldPosition(_roadSegment.Position);

            _material.SetFloat("_PositionZ", _transform.position.z);
        }


        private void OnOriginPositionRowChanged(float row)
        {
            ApplyPosition();
        }

        private void OnRoadSegmentLengthChanged(float length)
        {
            _material.SetFloat("_Progress", length);
        }
    }
}