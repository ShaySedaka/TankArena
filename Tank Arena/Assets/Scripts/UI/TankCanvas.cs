using Photon.Pun;
using TMPro;
using UnityEngine;

public class TankCanvas : MonoBehaviour
{
    [SerializeField] private TankHealthbar _tankHealthbar;

    [SerializeField] private TMP_Text _nickNameText;

    [SerializeField] private Tank _tank;

    [SerializeField] private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _tankHealthbar.Setup(_tank);
        _mainCamera = RoomManager.Instance.MainCamera;
        _nickNameText.text = _tank.PhotonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_mainCamera.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
