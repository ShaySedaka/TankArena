using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCanvas : MonoBehaviour
{
    [SerializeField] private TankHealthbar _tankHealthbar;

    [SerializeField] private Tank _tank;

    [SerializeField] private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _tankHealthbar.Setup(_tank);
        _mainCamera = RoomManager.Instance.MainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_mainCamera.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
