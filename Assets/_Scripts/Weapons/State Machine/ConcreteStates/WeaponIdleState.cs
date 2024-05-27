using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static PlayerController;

public class WeaponIdleState : WeaponState
{

    public WeaponIdleState(Weapon weapon, WeaponStateMachine weaponStateMachine) : base(weapon, weaponStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
