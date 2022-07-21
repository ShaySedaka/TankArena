using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class TopDownCharacterMover : MonoBehaviour
{
    [SerializeField]
    private Tank _tank;

    [SerializeField]
    private bool RotateTowardMouse;

    [SerializeField]
    private float RotationSpeed;

    [SerializeField]
    private Camera Camera;

    [SerializeField]
    private InputHandler _input;

    [SerializeField]
    private PhotonView _myPhotonView; 

    private void Start()
    {
        Camera = RoomManager.Instance.MainCamera;

        if(!_myPhotonView.IsMine)
        {
            Destroy(gameObject.GetComponentInChildren<Rigidbody>());
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveTank(_input.InputVector);


    }

    public void MoveTank(Vector3 direction)
    {
        var targetVector = new Vector3(direction.x, 0, direction.y).normalized;

        var movementVector = MoveTowardTarget(targetVector);

        if (!RotateTowardMouse)
        {
            RotateTowardMovementVector(movementVector);
        }
        if (RotateTowardMouse)
        {
            RotateFromMouseVector();
        }
    }

    private void RotateFromMouseVector()
    {
        Ray ray = Camera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = _tank.MovementSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, Camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if(movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}
