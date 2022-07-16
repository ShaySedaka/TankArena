using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _lifetimeInSeconds;
    [SerializeField] Gun _gunOrigin;
    [SerializeField] Collider _tankOriginCollider;
    [SerializeField] float _movementSpeed;

    private bool _didHitOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        //unparent the projectile to release it from the tank game object
        transform.parent = null;

        //Destroy the projectile in any case after lifetime expires
        StartCoroutine(DestroyAfterSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        //make the tank that was hit take damage from projectile
        if(_tankOriginCollider != other )
        {
            if (!_didHitOnce)
            {
                PhotonView _pv = other.gameObject.GetComponent<PhotonView>();
                if(_pv.IsMine)
                { other.gameObject.GetComponent<IDamagable>()?.TakeDamage(_gunOrigin.Damage); }              
                _didHitOnce = true; 
            }

            Destroy(gameObject);
        }
        
    }




    IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(_lifetimeInSeconds);
        Destroy(gameObject);
    }

    void MoveProjectile()
    {
        Vector3 direction = transform.forward;
        transform.position += direction * _movementSpeed * Time.deltaTime;

    }

    public void SetUp(Gun gun, Collider tankCollider)
    {
        _gunOrigin = gun;
        _tankOriginCollider = tankCollider;
    }
}
