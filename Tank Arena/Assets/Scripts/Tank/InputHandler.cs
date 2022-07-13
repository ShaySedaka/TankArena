using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviourPunCallbacks
{
    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    public bool IsAiming;

    [SerializeField] PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_view.IsMine)
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            InputVector = new Vector2(h, v);

            MousePosition = Input.mousePosition;

            IsAiming = Input.GetMouseButton(0);
        }


    }
}
