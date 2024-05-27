using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState
{
    protected Weapon weapon;
    protected WeaponStateMachine weaponStateMachine;
    protected Character character;

    public WeaponState(Weapon weapon, WeaponStateMachine weaponStateMachine)
    {
        this.weapon = weapon;
        this.weaponStateMachine = weaponStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { weapon.shootCooldown -= Time.deltaTime; }
    public virtual void PhysicsUpdate() { }
}
