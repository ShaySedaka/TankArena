using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    
    [SerializeField] float _smoothSpeed = 0.125f;
    [SerializeField] Vector3 _offset;

    private Transform _followTarget;


    private void FixedUpdate()
    {
        if(_followTarget == null)
        {
            return; 
        }

        Vector3 desiredPos = _followTarget.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

       // transform.LookAt(_followTarget);
    }

    public void SetFollowTarget(Transform target)
    {
        _followTarget = target;
    }
}
