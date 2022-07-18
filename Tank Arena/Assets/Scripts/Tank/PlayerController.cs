using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Tank _tank;

    public void StartRespawnCoroutine()
    {
        /* The coroutine for respawning the tank HAS to be called from outside of tank. 
         * Calling the coroutine from inside the tank stops it. Coroutines probably still rely on Unity's update
         * or some other callback system
         */
        StartCoroutine(_tank.RespawnMyControllerAfterDelay());
    }
}
