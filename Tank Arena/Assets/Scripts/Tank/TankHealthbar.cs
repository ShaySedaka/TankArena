using UnityEngine;
using UnityEngine.UI;

public class TankHealthbar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private Tank _tank;

    [SerializeField] private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = RoomManager.Instance.MainCamera;
        _slider.maxValue = _tank.MaxHelath;
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _tank.CurrentHelath;

        this.transform.LookAt(_mainCamera.transform);
    }


}
