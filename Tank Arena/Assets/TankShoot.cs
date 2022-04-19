using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{

    [SerializeField] GameObject _originProjectile;
    [SerializeField] GameObject _tankTurret;
    [SerializeField] GameObject _aimIndicator;
    [SerializeField] float _projectileLifetimeInSecs;
    [SerializeField] float _projectileSpeed;
    [SerializeField] Camera _mainCamera;

    private InputHandler _input;


    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.IsAiming)
        {
            if (_aimIndicator.activeSelf == false)
            {
                _aimIndicator.SetActive(true);
            }
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
