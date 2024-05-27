using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CharacterIdleState : CharacterState
{
    Vector2 _moveDir;
    float _scrollDir;

    public CharacterIdleState(Character character, CharacterStateMachine characterStateMachine) : base(character, characterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

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
        if (_moveDir != Vector2.zero)
        {
            characterStateMachine.ChangeState(character.MovingState);
        }

        _scrollDir = character.Controller.GetScrollDirection();
        if (_scrollDir == 0) return;
        character.ChangeWeapon(_scrollDir);
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
        character.CurrentWeapon._isShooting = true;
        character.CurrentWeapon.ChangeWeaponState(character.CurrentWeapon.ShootingState);
    }
    private void OnShootExit()
    {
        character.CurrentWeapon._isShooting = false;
        if (character.CurrentWeapon.StateMachine.CurrentWeaponState != character.CurrentWeapon.IdleState && character.CurrentWeapon.StateMachine.CurrentWeaponState != character.CurrentWeapon.ReloadingState)
            character.CurrentWeapon.ChangeWeaponState(character.CurrentWeapon.IdleState);
    }
    private void OnReload()
    {
        if (character.CurrentWeapon.CurrentAmmos != character.CurrentWeapon.MaxAmmos)
            character.CurrentWeapon.ChangeWeaponState(character.CurrentWeapon.ReloadingState);
    }

    public bool IsGrounded()
    {
        return (character.RB.velocity.y <= 0.1f && character.RB.velocity.y >= -0.1f);
    }
}
