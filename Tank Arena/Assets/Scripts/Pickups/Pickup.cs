using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private Transform _pickupManager;

    [SerializeField] protected Tank _tank;

    [SerializeField] private float _pickupDuration;

    private void Awake()
    {
        _pickupManager = PickupManager.Instance.gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {

        _tank = other.GetComponent<Tank>();
        if(_tank != null)
        {
            StartCoroutine(PickupBegin());
            transform.parent.transform.position = new Vector3(_pickupManager.position.x, _pickupManager.position.y, _pickupManager.position.z);
            transform.parent.gameObject.GetComponent<ObjectBounce>().StartHeight = _pickupManager.transform.position.y;
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
