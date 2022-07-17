using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOrb : Pickup
{
    [SerializeField] private int _scoreToAdd = 1;
    public override void ApplyPickUpEffect()
    {
        ScoreManager.Instance.AddScoreToTank(_tank.PhotonView, _scoreToAdd);
    }

    public override void RemovePickupEffect()
    {
        
    }
}
