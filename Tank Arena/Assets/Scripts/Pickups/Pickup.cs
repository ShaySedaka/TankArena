using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private Transform _pickupGraveyard;

    [SerializeField] protected Tank _tank;

    [SerializeField] private float _pickupDuration;

    private void Awake()
    {
        _pickupGraveyard = PickupManager.Instance.gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Sphere");
        _tank = other.GetComponent<Tank>();
        if(_tank != null)
        {
            StartCoroutine(PickupBegin());
            transform.parent.transform.position = new Vector3(_pickupGraveyard.position.x, _pickupGraveyard.position.y, _pickupGraveyard.position.z);


        }
    }

    public abstract void ApplyPickUpEffect();

    public abstract void RemovePickupEffect();

    public IEnumerator PickupBegin()
    {
        ApplyPickUpEffect();
        yield return new WaitForSeconds(_pickupDuration);
        RemovePickupEffect();
    }
}