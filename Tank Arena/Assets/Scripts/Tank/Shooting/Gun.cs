using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public float Damage;
    [SerializeField] protected int _shotRange = 10;

    public abstract override void Use();
}
