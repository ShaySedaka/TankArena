using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviourPunCallbacks
{
    public Vector2 InputVector { get; private set; }

    public Vector3 MousePosition { get; private set; }

    [SerializeField] TankShoot _shootScript;

    [SerializeField] PhotonView _view;


    // Update is called once per frame
    void Update()
    {
        if (_view.IsMine)
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            InputVector = new Vector2(h, v);

            MousePosition = Input.mousePosition;

            

            if(Input.GetMouseButtonDown(0))
            {
                _shootScript.IsAiming = Input.GetMouseButton(0);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                if(_shootScript.IsAiming)
                {
                    _shootScript.Shoot();
                }

                _shootScript.IsAiming = false;
            }
        }    
    }
}
