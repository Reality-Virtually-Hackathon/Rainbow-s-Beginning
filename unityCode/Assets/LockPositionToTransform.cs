using UnityEngine;

namespace Meta
{
    /// <summary>
    /// Locks this object's position to another object's transform position.
    /// </summary>
    public class LockPositionToTransform : MonoBehaviour
    {
        /// <summary>
        /// Should the position lock be enabled?
        /// </summary>
        [SerializeField]
        private bool _lockEnabled = true;

        /// <summary>
        /// Should use the main camera as the lockToTransform?
        /// </summary>
        [SerializeField]
        private bool _lockToMainCamera;

        /// <summary>
        /// The transform to lock the position to.
        /// </summary>
        [SerializeField]
        private Transform _lockToTransform;

        /// <summary>
        /// The offset to apply to the locked position.
        /// </summary>
        [SerializeField]
        private Vector3 _offset = Vector3.zero;

        public bool LockToMainCamera
        {
            get { return _lockToMainCamera; }
            set
            {
                _lockToMainCamera = value;

                if (_lockToMainCamera)
                {
                    _lockToTransform = Camera.main.transform;
                }
                else
                {
                    _lockToTransform = null;
                }
            }
        }

        /// <summary>
        /// The transform to lock the position to.
        /// </summary>
        public Transform LockToTransform
        {
            get { return _lockToTransform; }
            set { _lockToTransform = value; }
        }

        /// <summary>
        /// The offset to apply to the locked position.
        /// </summary>
        public Vector3 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private void Start()
        {
            if (_lockToMainCamera)
            {
                _lockToTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            UpdatePosition();
        }

        /// <summary>
        /// Activates the position lock and refresh the offset position.
        /// </summary>
        public void RenewLock()
        {
            _offset = transform.position - _lockToTransform.position;
            _lockEnabled = true;
        }

        /// <summary>
        /// Disables the position lock.
        /// </summary>
        public void DisableLock()
        {
            _lockEnabled = false;
        }

        /// <summary>
        /// Sets the position of this transform to the followed transform.
        /// </summary>
        private void UpdatePosition()
        {
            if (_lockToTransform != null && _lockEnabled)
            {
                transform.position = _lockToTransform.position + _offset;
            }
        }
    }
}

