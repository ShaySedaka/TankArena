using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _movementSpeed;
    private float _machineGunDMG;
    private float _machineGunFireRate;
    private float _machineGunRadius;

    private float _cannonDMG;
    private float _cannonCoolDown;
    private float _cannonProjectileSpeed;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public float MaxHelath { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHelath { get => _currentHealth; set => _currentHealth = value; }

    public void Shoot()
    {

    }

    public void Heal(float amount)
    {
        CurrentHelath = Mathf.Clamp(CurrentHelath + amount, 0, MaxHelath);
    }


}
