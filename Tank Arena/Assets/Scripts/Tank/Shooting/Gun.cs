using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public float Damage;
    [SerializeField] protected float _firingCooldown;
    [SerializeField] protected float _timeSinceLastShot;
    public bool CanShootGun { get => _timeSinceLastShot >= _firingCooldown; }
    [SerializeField] protected int _shotRange = 10;

    public abstract override void Use();
}
