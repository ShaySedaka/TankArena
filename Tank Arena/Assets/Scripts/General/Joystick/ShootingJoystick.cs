using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingJoystick : VirtualJoystick
{
    public void AddTankShooting(Tank tank)
    {
        _onTouchStart.AddListener(tank.TankShoot.LoadShell);
        _onTouchEnd.AddListener(tank.TankShoot.ReleaseShell);
        _onTouchHeld.AddListener(tank.TankShoot.RotateTurret);
    }
}
