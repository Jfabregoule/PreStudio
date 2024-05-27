using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CharacterMovingState : CharacterState
{
    float _scrollDir;

    [System.Serializable]
    public struct Descriptor
    {
        public Transform CameraTransform;
        public float Speed;
        public float MaxSpeed;
    }

    Descriptor _desc;

    public CharacterMovingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();
        character.Controller.onInteract += OnInteract;
        character.Controller.onJump += OnJump;
        character.Controller.onSprintEnter += OnSprintEnter;
        character.Controller.onShootEnter += OnShootEnter;
        character.Controller.onShootExit += OnShootExit;
        character.Controller.onReload += OnReload;
        character.Controller.onSwitchWeaponKeyboard += OnSwitchWeaponKeyboard;
        character.RB.drag = 2.5f;
    }

    public override void ExitState()
    {
        base.ExitState();
        character.Controller.onInteract -= OnInteract;
        character.Controller.onJump -= OnJump;
        character.Controller.onSprintEnter -= OnSprintEnter;
        character.Controller.onShootEnter -= OnShootEnter;
        character.Controller.onShootExit -= OnShootExit;
        character.Controller.onReload -= OnReload;
        character.Controller.onSwitchWeaponKeyboard -= OnSwitchWeaponKeyboard;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector2 moveDir = character.Controller.GetMoveDirection();

        if (moveDir == Vector2.zero)
        {
            characterStateMachine.ChangeState(character.IdleState);
            return;
        }

        Vector3 cameraForward = _desc.CameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 moveDirRelative = cameraForward * moveDir.y + _desc.CameraTransform.right * moveDir.x;
        moveDirRelative.Normalize();

        character.RB.AddForce(moveDirRelative * _desc.Speed, ForceMode.Force);

        Vector3 currentVel = character.RB.velocity;
        float yVelocity = character.RB.velocity.y;
        currentVel.y = 0;

        float currentSpeed = currentVel.magnitude;
        if (currentSpeed > _desc.MaxSpeed)
        {
            currentVel = currentVel.normalized * _desc.MaxSpeed;
        }
        currentVel.y = yVelocity;
        character.RB.velocity = currentVel;

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
    private void OnSprintEnter()
    {
        characterStateMachine.ChangeState(character.SprintingState);
    }
    private void OnShootEnter()
    {
        character.CurrentWeapon._isShooting = true;
        if (character.CurrentWeapon.StateMachine.CurrentWeaponState != character.CurrentWeapon.ReloadingState)
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
    private void OnSwitchWeaponKeyboard()
    {
        character.ChangeWeapon(character.Controller.GetNumKeyPressed());
    }
}
