using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public Camera MainCamera;
    [SerializeField] public List<Transform> SpawnPoints;
    [SerializeField] private int _clientTankSpawnPointIndex;
    [SerializeField] private Tank _localTank;

    public int PlayersSpawned = 0;

    public static RoomManager Instance;

    public PhotonView PhotonView;

    public int ClientTankSpawnPointIndex { get => _clientTankSpawnPointIndex;}
    public Tank LocalTank { get => _localTank; set => _localTank = value; }

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
            Instance = this;
    }

    public void AssignSpawnPoints()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            int spawnPointIndex = 0;
            foreach (KeyValuePair<int, int> entry in ScoreManager.Instance.PlayerScores)
            {
                PhotonView.RPC("RPC_AssignSpawnPoint", RpcTarget.All, entry.Key, spawnPointIndex);
                spawnPointIndex++;    
                
            }
        }
    }

    [PunRPC]
    private void RPC_AssignSpawnPoint(int viewID, int spawnPointIndex)
    {
        if(viewID == _localTank.PhotonView.ViewID)
        {
            _clientTankSpawnPointIndex = spawnPointIndex;
            Debug.Log("I got index: " + _clientTankSpawnPointIndex);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();

    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }

    #region Photon Callbacks
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        Debug.Log("Total Players in Room: " + PhotonNetwork.PlayerList.Length);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    #endregion
}
