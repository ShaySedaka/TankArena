using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup{
    public override void ApplyPickUpEffect()
    {
        _tank.Heal(_tank.MaxHelath * 0.5f);
    }

    public override void RemovePickupEffect()
    {
        
    }
}
