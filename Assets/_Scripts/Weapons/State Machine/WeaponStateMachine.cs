using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateMachine
{
    public WeaponState CurrentWeaponState { get; set; }

    public void Initialize(WeaponState startingState)
    {
        CurrentWeaponState = startingState;
        CurrentWeaponState.EnterState();
    }

    public void ChangeState(WeaponState newState)
    {
        CurrentWeaponState.ExitState();
        CurrentWeaponState = newState;
        CurrentWeaponState.EnterState();
    }
}
