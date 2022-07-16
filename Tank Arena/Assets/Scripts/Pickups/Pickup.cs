using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private PickupManager _pickupManager;

    [SerializeField] protected Tank _tank;

    [SerializeField] private float _pickupDuration;

    [SerializeField] private int _spawnPointIndex;

    private void Awake()
    {
        _pickupManager = PickupManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {

        _tank = other.GetComponent<Tank>();
        if(_tank != null)
        {
            StartCoroutine(PickupBegin());
            transform.parent.transform.position = new Vector3(_pickupManager.transform.position.x, _pickupManager.transform.position.y, _pickupManager.transform.position.z);
            transform.parent.gameObject.GetComponent<ObjectBounce>().StartHeight = _pickupManager.transform.position.y;
        }
    }

    public abstract void ApplyPickUpEffect();

    public abstract void RemovePickupEffect();

    public IEnumerator PickupBegin()
    {
        ApplyPickUpEffect();
        ClearSpawnPointIndex();
        yield return new WaitForSeconds(_pickupDuration);
        RemovePickupEffect();
        yield return new WaitForSeconds(3);
        //Destroy(transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);

    }
    public void Setup(int index)
    {
        _spawnPointIndex = index;
    }

    private void ClearSpawnPointIndex()
    {
        _pickupManager.FreeSpawnPoint(_spawnPointIndex);
    }
}
