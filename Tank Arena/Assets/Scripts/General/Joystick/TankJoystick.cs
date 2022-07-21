using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankJoystick : VirtualJoystick
{
    [SerializeField] private TopDownCharacterMover _tankMover;

    protected override void TheBetterStart()
    {
        //StartCoroutine(FindTankMover());
    }

    protected override void OnJoystickHeld(Vector3 direction)
    {
        RoomManager.Instance.LocalTank.TankMover.MoveTank(direction);
    }

    protected override void OnJoystickPressed(Vector3 direction)
    {
        
    }

    protected override void OnJoystickReleased(Vector3 direction)
    {
        
    }

    private IEnumerator FindTankMover()
    {
        while(RoomManager.Instance.LocalTank.TankMover == null)
        {
            yield return null;
        }
        _tankMover = RoomManager.Instance.LocalTank.TankMover;
    }
}
