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
            CreatePlayerController();
            _cameraFollowScript = RoomManager.Instance.MainCamera.GetComponent<CameraFollow>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayerController()
    {
        //Instantiate Player Controller
       _instantiatedController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);

        AssignCameraFollowToPlayerController();
    }

    private void AssignCameraFollowToPlayerController()
    {
        
       _cameraFollowScript.SetFollowTarget(_instantiatedController.transform);
    }
}
