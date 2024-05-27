using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    //[SerializeField] private WeaponShootingState.Descriptor _gunShootingStateDescriptor;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    ShootingState = new WeaponShootingState(this, StateMachine, _gunShootingStateDescriptor);
    //}

    //public override void Shoot()
    //{
    //    // Implémenter le comportement de tir spécifique au Gun
    //    if (CurrentAmmos > 0)
    //    {
    //        GameObject.Instantiate(projectilePrefab, transform.position + transform.forward, Quaternion.identity);
    //        CurrentAmmos--;
    //        shootCooldown = 1f / _gunShootingStateDescriptor.fireRate;
    //    }
    //}
}
