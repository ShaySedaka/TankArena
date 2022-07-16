using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOrb : Pickup
{
    public override void ApplyPickUpEffect()
    {
        Debug.Log("Add Score");
    }

    public override void RemovePickupEffect()
    {
        
    }
}
