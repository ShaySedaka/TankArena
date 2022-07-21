using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankJoystick : VirtualJoystick
{
    public void AddTankMoving(Tank tank)
    {
        _onTouchHeld.AddListener(tank.TankMover.MoveTank);
    }
}
