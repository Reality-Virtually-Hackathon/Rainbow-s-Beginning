using UnityEngine;
using UnityEngine.Serialization;

namespace Meta
{
    /// <summary>
    /// Sets the rotation of this object to match the rotation of another.
    /// </summary>
    public class LockRotationToTransform : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("_sourceTransform")]
        private Transform _lockToTransform;

        [SerializeField]
        private bool _lock;

        [SerializeField]
        private bool _lockToMainCamera;

        public Transform LockToTransform
        {
            get { return _lockToTransform; }
            set { _lockToTransform = value; }
        }

        public bool Lock
        {
            get { return _lock; }
            set { _lock = value; }
        }

        private void Awake()
        {
            if (_lockToMainCamera)
            {
                _lockToTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (_lock)
            {
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            transform.rotation = _lockToTransform.rotation;
        }
    }
}