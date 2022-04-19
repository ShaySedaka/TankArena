using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform _followTarget;
    [SerializeField] float _smoothSpeed = 0.125f;
    [SerializeField] Vector3 _offset;


    private void FixedUpdate()
    {
        Vector3 desiredPos = _followTarget.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

       // transform.LookAt(_followTarget);
    }

}
