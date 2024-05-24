using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public bool IsDamageable { get; set; } = true;

    public PlayerController Controller;
    public Rigidbody RB;

    [SerializeField] private CharacterMovingState.Descriptor _movingStateDescriptor;

    #region State Machine Variables

    public CharacterStateMachine StateMachine { get; set; }
    public CharacterIdleState IdleState { get; set; }
    public CharacterMovingState MovingState { get; set; }
    public CharacterDyingState DyingState { get; set; }

    #endregion

    private void Awake()
    {
        StateMachine = new CharacterStateMachine();
        IdleState = new CharacterIdleState(this, StateMachine);
        DyingState = new CharacterDyingState(this, StateMachine);
        MovingState = new CharacterMovingState(this, StateMachine, _movingStateDescriptor);
    }

    private void Start()
    {
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(IdleState);
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
}