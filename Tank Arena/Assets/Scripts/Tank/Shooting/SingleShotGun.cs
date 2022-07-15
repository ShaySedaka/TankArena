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
        if(Physics.Raycast(ray, out RaycastHit hit, _shotRange))
        {
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Damage);
        }

        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_shootingOriginPoint.position, _shootingOriginPoint.forward* _shotRange);
    }
}
