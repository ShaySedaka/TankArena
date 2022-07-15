using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviourPunCallbacks, IDamagable
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Outline _outline;
    [SerializeField] private TankHealthbar _tankHealthbar;

    private float _machineGunDMG;
    private float _machineGunFireRate;
    private float _machineGunRadius;

    private float _cannonDMG;
    private float _cannonCoolDown;
    private float _cannonProjectileSpeed;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public float MaxHelath { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; 
        set
        {
            _currentHealth = value;
            _tankHealthbar.UpdateHealthBar();
        }  
    }


    private void Start()
    {
        if(_photonView.IsMine)
        {
            _outline.enabled = false;
        }
    }

    public void Shoot()
    {

    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHelath);
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(!_photonView.IsMine && targetPlayer == _photonView.Owner)
        {

        }
    }
    
    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);


        if(CurrentHealth == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("A Tank died today. Shame.");
    }
}
