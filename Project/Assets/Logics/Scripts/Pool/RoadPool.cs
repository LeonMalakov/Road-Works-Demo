using System.Collections.Generic;
using UnityEngine;

namespace Combine
{
    public class RoadPool : MonoBehaviour
    {
        [SerializeField] private GameObject _roadSegmentPrefab;

        // Anchor for disabled segments.
        private Transform _poolParent;

        private Stack<RoadSegment> _segments = new Stack<RoadSegment>();


        private void Awake()
        {
            // Get anchor reference and disable.
            _poolParent = new GameObject("RoadSegments").transform;
            _poolParent.SetParent(transform);
            _poolParent.gameObject.SetActive(false);
        }


        public RoadSegment Spawn(Transform parent)
        {
            RoadSegment segment;

            if (_segments.Count > 0)
                segment = _segments.Pop();
            else
                segment = InstantiateNew();

            segment.transform.SetParent(parent);
            segment.ResetObject();

            return segment;
        }

        public void Despawn(RoadSegment segment)
        {
            segment.transform.SetParent(_poolParent);

            _segments.Push(segment);
        }


        private RoadSegment InstantiateNew()
        {
            return Instantiate(_roadSegmentPrefab).GetComponent<RoadSegment>();
        }
    }
}