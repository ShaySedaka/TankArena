using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] CameraFollow _cameraFollowScript;
    


    private PhotonView _photonView;
    private GameObject _instantiatedController;
    


    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_photonView.IsMine)
        {
            _cameraFollowScript = RoomManager.Instance.MainCamera.GetComponent<CameraFollow>();
            CreatePlayerController();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayerController()
    {
        RoomManager roomManager = RoomManager.Instance;

        Debug.Log("Creating player controller");

        int playerIndex = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i].UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                playerIndex = i;
            }
        }

        //Instantiate Player Controller
        _instantiatedController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), 
        roomManager.SpawnPoints[playerIndex].position, Quaternion.identity, 0, new object[] { _photonView.ViewID });
        roomManager.LocalTank = _instantiatedController.GetComponentInChildren<Tank>();
        _photonView.RPC("RPC_TankCreated", RpcTarget.All, _instantiatedController.GetComponentInChildren<PhotonView>().ViewID);
        AssignCameraFollowToPlayerController();
    }

    private void AssignCameraFollowToPlayerController()
    {
        
       _cameraFollowScript.SetFollowTarget(_instantiatedController.GetComponentInChildren<Rigidbody>().transform);
    }

    [PunRPC]
    private void RPC_TankCreated(int viewID)
    {
        ScoreManager.Instance.AddTankToScoreBoard(viewID);
    }

}
