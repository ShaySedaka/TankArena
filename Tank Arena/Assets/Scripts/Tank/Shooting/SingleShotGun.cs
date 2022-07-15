using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SingleShotGun : Gun
{
    [SerializeField] Transform _shootingOriginPoint;
    [SerializeField] PhotonView _photonView;
    [SerializeField] GameObject _projectile;
    [SerializeField] Collider _tankCollider;
    
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
            //hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Damage);  
        }

        _photonView.RPC("RPC_Shoot", RpcTarget.All, _photonView.ViewID);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_shootingOriginPoint.position, _shootingOriginPoint.forward* _shotRange);
    }

    [PunRPC]
    void RPC_Shoot(int shooterViewID)
    {
        if(_photonView.ViewID == shooterViewID)
        {
            GameObject projectile = Instantiate(_projectile, _shootingOriginPoint);
            projectile.GetComponent<Projectile>().SetUp(this, _tankCollider);
        }
    }
}
