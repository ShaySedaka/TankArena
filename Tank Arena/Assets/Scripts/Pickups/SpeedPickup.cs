using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : Pickup
{
    [SerializeField] private float _speedFactor = 2;

    public override void ApplyPickUpEffect()
    {
        _tank.MovementSpeed *= _speedFactor;
    }

    public override void RemovePickupEffect()
    {
        Debug.Log("Removing Speed");
        _tank.MovementSpeed /= _speedFactor;
    }
}
