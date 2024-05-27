using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CharacterIdleState : CharacterState
{
    const string SPEED_PARAM = "Speed";

    Vector2 _moveDir;

    public CharacterIdleState(Character character, CharacterStateMachine characterStateMachine) : base(character, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Vector2 moveDir = character.Controller.GetMoveDirection();

        character.Controller.onInteract += OnInteract;
        character.Controller.onJump += OnJump;
        character.Controller.onShootEnter += OnShootEnter;
        character.Controller.onShootExit += OnShootExit;
        character.Controller.onReload += OnReload;
        character.RB.drag = 2.5f;
    }

    public override void ExitState()
    {
        base.ExitState();

        character.Controller.onInteract -= OnInteract;
        character.Controller.onJump -= OnJump;
        character.Controller.onShootEnter -= OnShootEnter;
        character.Controller.onShootExit -= OnShootExit;
        character.Controller.onReload -= OnReload;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        _moveDir = character.Controller.GetMoveDirection();
        if (_moveDir == Vector2.zero) return;

        characterStateMachine.ChangeState(character.MovingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    private void OnInteract()
    {
        Vector3 playerPosition = character.transform.position;
        Vector3 forwardDirection = character.transform.forward;

        float interactionRange = 3f;
        float sphereRadius = 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(playerPosition, sphereRadius, forwardDirection, interactionRange);

        foreach (RaycastHit hit in hits)
        {
            Vector3 directionToHit = hit.point - playerPosition;

            float angle = Vector3.Angle(forwardDirection, directionToHit);
            if (angle <= 45f)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    break;
                }
            }
        }
    }
    private void OnJump()
    {
        if (IsGrounded())
        {
            characterStateMachine.ChangeState(character.JumpingState);
        }
    }
    private void OnShootEnter()
    {
        character.Weapon._isShooting = true;
        character.Weapon.ChangeWeaponState(character.Weapon.ShootingState);
    }
    private void OnShootExit()
    {
        character.Weapon._isShooting = false;
        if (character.Weapon.StateMachine.CurrentWeaponState != character.Weapon.IdleState && character.Weapon.StateMachine.CurrentWeaponState != character.Weapon.ReloadingState)
            character.Weapon.ChangeWeaponState(character.Weapon.IdleState);
    }
    private void OnReload()
    {
        if (character.Weapon.CurrentAmmos != character.Weapon.MaxAmmos)
            character.Weapon.ChangeWeaponState(character.Weapon.ReloadingState);
    }

    public bool IsGrounded()
    {
        return (character.RB.velocity.y <= 0.1f && character.RB.velocity.y >= -0.1f);
    }
}
