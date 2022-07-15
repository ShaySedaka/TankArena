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

        //Instantiate Player Controller
       _instantiatedController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), 
           roomManager.SpawnPoints[(roomManager.PlayersSpawned++)% (roomManager.SpawnPoints.Count)].position, Quaternion.identity, 0, new object[] { _photonView.ViewID });
        

        AssignCameraFollowToPlayerController();
    }

    private void AssignCameraFollowToPlayerController()
    {
        
       _cameraFollowScript.SetFollowTarget(_instantiatedController.GetComponentInChildren<Rigidbody>().transform);
    }

}