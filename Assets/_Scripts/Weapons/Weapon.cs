using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int MaxAmmos { get; set; } = 6;
    public int CurrentAmmos { get; set; } = 6;

    [SerializeField] private WeaponShootingState.Descriptor _shootingStateDescriptor;
    [SerializeField] private WeaponReloadingState.Descriptor _reloadingStateDescriptor;

    [HideInInspector]
    public bool _isShooting = false;

    [HideInInspector]
    public float shootCooldown = 0;

    public GameObject projectilePrefab; 
    public Transform cameraTransform;

    #region State Machine Variables

    public WeaponStateMachine StateMachine { get; set; }
    public WeaponIdleState IdleState { get; set; }
    public WeaponReloadingState ReloadingState { get; set; }
    public WeaponShootingState ShootingState { get; set; }

    #endregion

    protected virtual void Awake()
    {
        StateMachine = new WeaponStateMachine();
        IdleState = new WeaponIdleState(this, StateMachine);
        ReloadingState = new WeaponReloadingState(this, StateMachine, _reloadingStateDescriptor);
        ShootingState = new WeaponShootingState(this, StateMachine, _shootingStateDescriptor);
    }

    protected virtual void Start()
    {
        CurrentAmmos = MaxAmmos;
        StateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        Debug.Log(StateMachine.CurrentWeaponState);
        StateMachine.CurrentWeaponState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentWeaponState.PhysicsUpdate();
    }

    public void ChangeWeaponState(WeaponState state)
    {
        StateMachine.ChangeState(state);
    }

    public virtual void Shoot()
    {
        if (CurrentAmmos > 0)
        {
            GameObject.Instantiate(projectilePrefab, transform.position + transform.forward, cameraTransform.rotation);
            CurrentAmmos--;

            if (CurrentAmmos > 0) return;

            StateMachine.ChangeState(ReloadingState);
        }
    }
}
