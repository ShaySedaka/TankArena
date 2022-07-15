using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Outline _outline;
    [SerializeField] private TankHealthbar _tankHealthbar;


    [SerializeField] private PlayerManager _playerManager;
    private float _machineGunDMG;
    private float _machineGunFireRate;
    private float _machineGunRadius;

    private float _cannonDMG;
    private float _cannonCoolDown;
    private float _cannonProjectileSpeed;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public float MaxHelath { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; 
        set
        {
            _currentHealth = value;
            _tankHealthbar.UpdateHealthBar();
        }
    }

    public PlayerManager PlayerManager { get => _playerManager; set => _playerManager = value; }

    private void Awake()
    {
        // find the associated PlayerManagerScript
        PlayerManager = PhotonView.Find((int)_photonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if(_photonView.IsMine)
        {
            _outline.enabled = false;
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHelath);
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!_photonView.IsMine && targetPlayer == _photonView.Owner)
        {

        }
    }



    #region Damage Management

    public void TakeDamage(float damage)
    {
        _photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage, _photonView.ViewID); 

    }

    public void Die(int viewIDWhoDied)
    {
        Debug.Log("A Tank died today. Shame.");

        Debug.Log("Rewpawning Tank");
        RespawnController(viewIDWhoDied);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, int viewID)
    {
        if(_photonView.ViewID == viewID)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
             
            if (CurrentHealth == 0)
            {
                Die(viewID);
            }
        }
    }



    #endregion

    #region Respawn


    public void RespawnController(int controllerViewID)
    {
        //Make my camera follow the person who killed me (put some transform here)
        //Disable my player controller
        _photonView.RPC("RPC_SetMyControllerActivity", RpcTarget.All, _photonView.ViewID, false);
        //Wait for X seconds until my respawn

        //Reset to full health
        _photonView.RPC("RPC_RefillTankHealth", RpcTarget.All, _photonView.ViewID);

        //Move my controller to another spawn point
        _photonView.RPC("RPC_MoveMyControllerToAnotherSpawn", RpcTarget.All, _photonView.ViewID);
        //Enable my player controller
        _photonView.RPC("RPC_SetMyControllerActivity", RpcTarget.All, _photonView.ViewID, true);
        //Make my camera follow me again
    }


    [PunRPC]
    public void RPC_SetMyControllerActivity(int controllerViewID, bool activity)
    {
        if (_photonView.ViewID == controllerViewID)
        {
            gameObject.SetActive(activity);
        }
    }

    [PunRPC]
    public void RPC_MoveMyControllerToAnotherSpawn(int controllerViewID)
    {
        if(_photonView.ViewID == controllerViewID)
        {
            System.Random r = new System.Random();
            int spawnIndex = r.Next(4);

            List<Transform> spawnPoints = RoomManager.Instance.SpawnPoints;

            transform.parent.transform.position = spawnPoints[2].position;
            transform.localPosition = Vector3.zero;
        }
    }

    [PunRPC]
    public void RPC_RefillTankHealth(int controllerViewID)
    {
        if (_photonView.ViewID == controllerViewID)
        {
            CurrentHealth = MaxHelath;
        }
    }
    #endregion
}
