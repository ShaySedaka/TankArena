using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePickup : Pickup
{
    [SerializeField] private float _damageBonus = 2;

    public override void ApplyPickUpEffect()
    {
        if (_tank.photonView.IsMine)
        {
            _tank.Canon.Damage += _damageBonus;
        }
    }

    public override void RemovePickupEffect()
    {
        if (_tank.photonView.IsMine)
        {
            _tank.Canon.Damage -= _damageBonus;
        }
    }
}
