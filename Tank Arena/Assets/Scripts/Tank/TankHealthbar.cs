using UnityEngine;
using UnityEngine.UI;

public class TankHealthbar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Tank _tank;

    void Start()
    {
        _slider.maxValue = _tank.MaxHelath;
    }

    public void UpdateHealthBar()
    {
        _slider.value = _tank.CurrentHealth;
    }

    public void Setup(Tank tank)
    {
        _tank = tank;
    }

}
