using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static PlayerController;

public class WeaponReloadingState : WeaponState
{ 

    [System.Serializable]
    public struct Descriptor
    {
        public float reloadingTime;
    }

    Descriptor _desc;
    public WeaponReloadingState(Weapon weapon, WeaponStateMachine weaponStateMachine, Descriptor desc) : base(weapon, weaponStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();
        weapon.StartCoroutine(Reload());
    }

    public override void ExitState()
    {
        base.ExitState();
        weapon.CurrentAmmos = weapon.MaxAmmos;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_desc.reloadingTime);
        if (weapon._isShooting == false) 
        {
            weaponStateMachine.ChangeState(weapon.IdleState);
        }
        else
        {
            weaponStateMachine.ChangeState(weapon.ShootingState);
        }
        
    }
}
