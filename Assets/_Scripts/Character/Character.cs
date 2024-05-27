using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Character : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public bool IsDamageable { get; set; } = true;

    public PlayerController Controller;
    public Rigidbody RB;
    public List<Weapon> Weapons;
    [HideInInspector]
    public Weapon CurrentWeapon;

    private float weaponSwitchCooldown = 0.2f;
    private float lastWeaponSwitchTime;

    [SerializeField] private CharacterMovingState.Descriptor _movingStateDescriptor;
    [SerializeField] private CharacterSprintingState.Descriptor _sprintingStateDescriptor;
    [SerializeField] private CharacterJumpingState.Descriptor _jumpingStateDescriptor;
    [SerializeField] private CharacterAirBornState.Descriptor _airBornStateDescriptor;

    #region State Machine Variables

    public CharacterStateMachine StateMachine { get; set; }
    public CharacterIdleState IdleState { get; set; }
    public CharacterMovingState MovingState { get; set; }
    public CharacterSprintingState SprintingState { get; set; }
    public CharacterJumpingState JumpingState { get; set; }
    public CharacterAirBornState AirBornState { get; set; }
    public CharacterDyingState DyingState { get; set; }

    #endregion

    private void Awake()
    {
        StateMachine = new CharacterStateMachine();
        IdleState = new CharacterIdleState(this, StateMachine);
        DyingState = new CharacterDyingState(this, StateMachine);
        MovingState = new CharacterMovingState(this, StateMachine, _movingStateDescriptor);
        SprintingState = new CharacterSprintingState(this, StateMachine, _sprintingStateDescriptor);
        JumpingState = new CharacterJumpingState(this, StateMachine, _jumpingStateDescriptor);
        AirBornState = new CharacterAirBornState(this, StateMachine, _airBornStateDescriptor);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentWeapon = Weapons[0];
        StateMachine.Initialize(IdleState);
        lastWeaponSwitchTime = -weaponSwitchCooldown;
    }

    private void Update()
    {
        StateMachine.CurrentCharacterState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentCharacterState.PhysicsUpdate();
    }

    public bool Damage(float amount)
    {
        if (!IsDamageable) return false;
        if (StateMachine.CurrentCharacterState == DyingState) return false;

        CurrentHealth -= amount;

        if (CurrentHealth < 0f)
        {
            Die();
        }
        return true;
    }

    public void Die()
    {
        StateMachine.ChangeState(DyingState);
    }

    private IEnumerator Quit(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }

    public void ChangeCharacterState(CharacterState state)
    {
        StateMachine.ChangeState(state);
    }

    public void ChangeWeapon(float side)
    {
        if (Time.time - lastWeaponSwitchTime >= weaponSwitchCooldown)
        {
            lastWeaponSwitchTime = Time.time;

            if (side == 1)
            {
                if (CurrentWeapon == Weapons[Weapons.Count - 1])
                    CurrentWeapon = Weapons[0];
                else
                    CurrentWeapon = Weapons[Weapons.IndexOf(CurrentWeapon) + 1];
            }
            else if (side == -1)
            {
                if (CurrentWeapon == Weapons[0])
                    CurrentWeapon = Weapons[Weapons.Count - 1];
                else
                    CurrentWeapon = Weapons[Weapons.IndexOf(CurrentWeapon) - 1];
            }
        }
    }

    public void ChangeWeapon(int slot)
    {
        if (Time.time - lastWeaponSwitchTime >= weaponSwitchCooldown)
        {
            Debug.Log(slot);
            lastWeaponSwitchTime = Time.time;

            if (slot < Weapons.Count && Weapons[slot] != null)
            {
                CurrentWeapon = Weapons[slot];
            }
        }
    }
}
