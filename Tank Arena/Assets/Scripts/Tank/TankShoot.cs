using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField] private Color _yellowColor;
    [SerializeField] private Color _redColor;

    [SerializeField] private GameObject _tankTurret;
    [SerializeField] private GameObject _aimIndicator;
    [SerializeField] private Renderer _aimIndicatorRenderer;
    [SerializeField] private InputHandler _input;

    [SerializeField] private Gun _cannonWeapon;

    private Camera _mainCamera;

    public bool IsAiming { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = RoomManager.Instance.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAiming)
        {
            if (_aimIndicator.activeSelf == false)
            {
                _aimIndicator.SetActive(true);
            }
            ChangeAimindicatorColor();
            RotateFromMouseVector(_tankTurret);
        }
        else
        {
            if (_aimIndicator.activeSelf == true)
            {
                _aimIndicator.SetActive(false);
            }
        }
    }

    private void ChangeAimindicatorColor()
    {
        if (_cannonWeapon.CanShootGun)
        {
            _aimIndicatorRenderer.material.SetColor("_Color", _yellowColor);
        }
        else
        {
            _aimIndicatorRenderer.material.SetColor("_Color", _redColor);
        }
    }

    public void Shoot()
    {
        _cannonWeapon.Use();
    }


    private void RotateFromMouseVector(GameObject objectToRotate)
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = objectToRotate.transform.position.y;
            objectToRotate.transform.LookAt(target);
        }
    }

}
