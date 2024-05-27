using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootingState : WeaponState
{
    [System.Serializable]
    public struct Descriptor
    {
        public float damage;
        public float fireRate;
    }

    Descriptor _desc;

    public WeaponShootingState(Weapon weapon, WeaponStateMachine weaponStateMachine, Descriptor desc) : base(weapon, weaponStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();
        if (weapon.shootCooldown <= 0f)
        {
            weapon.Shoot();
            weapon.shootCooldown = 1f / _desc.fireRate;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (weapon.shootCooldown <= 0f)
        {
            weapon.Shoot();
            weapon.shootCooldown = 1f / _desc.fireRate;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
