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
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private TopDownCharacterMover _tankMover;

    [SerializeField] private GameObject _tankSihlouette;

    [SerializeField] private float _respawnTime;

    [SerializeField] private PlayerManager _playerManager;
    private float _machineGunDMG;
    private float _machineGunFireRate;
    private float _machineGunRadius;

    [SerializeField] private Gun _canon;

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
    public Gun Canon { get => _canon; set => _canon = value; }
    public PhotonView PhotonView { get => _photonView; }
    public float RespawnTime { get => _respawnTime; }
    public InputHandler InputHandler { get => _inputHandler; set => _inputHandler = value; }
    public TopDownCharacterMover TankMover { get => _tankMover; }

    private void Awake()
    {
        // find the associated PlayerManagerScript
        PlayerManager = PhotonView.Find((int)PhotonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if(PhotonView.IsMine)
        {
            _outline.enabled = false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!PhotonView.IsMine && targetPlayer == PhotonView.Owner)
        {

        }
    }

    public void DisablePlayerMovement()
    {
        _inputHandler.enabled = false;
        TankMover.enabled = false;
    }

    #region Damage Management

    public void Heal(float healAmount)
    {
        PhotonView.RPC("RPC_Heal", RpcTarget.All, healAmount, PhotonView.ViewID);
    }

    [PunRPC]
    private void RPC_Heal(float healAmount, int viewID)
    {
        if (PhotonView.ViewID == viewID)
        {
            CurrentHealth = Mathf.Min(MaxHelath, CurrentHealth + healAmount);
        }
    }

    public void TakeDamage(float damage, int shooterViewID)
    {
        PhotonView.RPC("RPC_TakeDamage", RpcTarget.All, damage, PhotonView.ViewID, shooterViewID); 

    }

    [PunRPC]
    private void RPC_TakeDamage(float damage, int viewID, int shooterViewID)
    {
        if(PhotonView.ViewID == viewID)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

            PhotonView shooterPV = PhotonView.Find(shooterViewID);
            ScoreManager.Instance.AddScoreForHit(shooterPV);
            if (CurrentHealth == 0)
            {
                Die(viewID, shooterPV);
            }
        }
    }

    public void Die(int viewIDWhoDied, PhotonView shooterPV)
    {
        ScoreManager.Instance.AddScoreForKill(shooterPV);
        RespawnController();
    }


    #endregion

    #region Respawn


    public void RespawnController()
    {
        //Make my camera follow the person who killed me (put some transform here)

        // Disable the tank, and respawn it after the respawn timer passes
        _playerController.StartRespawnCoroutine();

        // Present Respawn Timer for dead player
        PhotonView.RPC("RPC_PresentRespawnTimer", RpcTarget.All, PhotonView.ViewID);

        //Reset to full health
        PhotonView.RPC("RPC_RefillTankHealth", RpcTarget.All, PhotonView.ViewID);

        //Move my controller to another spawn point   
        System.Random rand = new System.Random();
        int randomSpawnPointIndex = rand.Next(RoomManager.Instance.SpawnPoints.Count);
        PhotonView.RPC("RPC_MoveMyControllerToAnotherSpawn", RpcTarget.All, PhotonView.ViewID, randomSpawnPointIndex);

        //Make my camera follow me again
    }

    public IEnumerator RespawnMyControllerAfterDelay()
    {
        PhotonView.RPC("RPC_SetMyControllerActivity", RpcTarget.All, PhotonView.ViewID, false);

        yield return new WaitForSeconds(RespawnTime);

        PhotonView.RPC("RPC_SetMyControllerActivity", RpcTarget.All, PhotonView.ViewID, true);
    }

    [PunRPC]
    private void RPC_PresentRespawnTimer(int controllerViewID)
    {
        if(PhotonView.ViewID == controllerViewID && PhotonView.IsMine)
        {
            GameUIManager.Instance.RespawnPopup.Present();
        }
    }

    [PunRPC]
    public void RPC_SetMyControllerActivity(int controllerViewID, bool activity)
    {
        if (PhotonView.ViewID == controllerViewID)
        {
            gameObject.SetActive(activity);
            if(PhotonView.IsMine)
            {
                _tankSihlouette.SetActive(!activity);
            }
        }
    }

    [PunRPC]
    public void RPC_MoveMyControllerToAnotherSpawn(int controllerViewID, int spawnPointIndex)
    {
        if(PhotonView.ViewID == controllerViewID)
        {
            List<Transform> spawnPoints = RoomManager.Instance.SpawnPoints;

            transform.position = spawnPoints[spawnPointIndex].position;
            transform.rotation = Quaternion.identity;
            if (PhotonView.IsMine)
            {
                _tankSihlouette.transform.position = spawnPoints[spawnPointIndex].position;
            }

            //transform.parent.transform.position = spawnPoints[spawnPointIndex].position;
            //transform.localPosition = Vector3.zero;
        }
    }

    [PunRPC]
    public void RPC_RefillTankHealth(int controllerViewID)
    {
        if (PhotonView.ViewID == controllerViewID)
        {
            CurrentHealth = MaxHelath;
        }
    }
    #endregion
}
