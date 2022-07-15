using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Transform _shootingOriginPoint;


    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Camera _mainCam = RoomManager.Instance.MainCamera;

        Ray ray = new Ray(_shootingOriginPoint.position, _shootingOriginPoint.forward);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
           // hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage();
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_shootingOriginPoint.position, _shootingOriginPoint.forward*5);
    }
}
