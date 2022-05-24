using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    float _helath;
    [SerializeField] private float _movementSpeed;
    private float _machineGunDMG;
    private float _machineGunFireRate;
    private float _machineGunRadius;

    private float _cannonDMG;
    private float _cannonCoolDown;
    private float _cannonProjectileSpeed;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }

    public void Shoot()
    {

    }
}
