using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;

public class ToggleMetaLocking : MonoBehaviour {
    [SerializeField]
    private LockPositionToTransform _positionLock;

    [SerializeField]
    private LockRotationToTransform _rotationLock;

    [SerializeField]
    private bool ShouldLockInitially;

    private bool _currentLockState;

    private void Awake()
    {
        if (_positionLock== null || _rotationLock == null)
        {
            Debug.Log("ENsure that metalocking attached to this cscirpt");
        }
        _currentLockState = ShouldLockInitially;
        SetLock(_currentLockState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetLock(_currentLockState);
            _currentLockState = !_currentLockState;
        }
    }

    private void SetLock(bool setlock)
    {
        if (setlock)
        {
            _positionLock.RenewLock();
            _rotationLock.Lock = true;
        }else
        {
            _positionLock.DisableLock();
            _rotationLock.Lock = false;
        }
    }

}
