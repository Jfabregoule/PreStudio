using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CharacterJumpingState : CharacterState
{
    float _scrollDir;

    [System.Serializable]
    public struct Descriptor
    {
        public float jumpHeight;
        public Transform CameraTransform;
        public float Speed;
        public float MaxSpeed;
    }

    Descriptor _desc;

    public CharacterJumpingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();
        character.Controller.onInteract += OnInteract;
        character.Controller.onShootEnter += OnShootEnter;
        character.Controller.onShootExit += OnShootExit;
        character.Controller.onReload += OnReload;
        character.Controller.onSwitchWeaponKeyboard += OnSwitchWeaponKeyboard;

        character.RB.drag = 0;
        character.RB.AddForce(new Vector3(0.0f, _desc.jumpHeight, 0.0f), ForceMode.Impulse);
    }

    public override void ExitState()
    {
        base.ExitState();
        character.Controller.onInteract -= OnInteract;
        character.Controller.onShootEnter -= OnShootEnter;
        character.Controller.onShootExit -= OnShootExit;
        character.Controller.onReload -= OnReload;
        character.Controller.onSwitchWeaponKeyboard -= OnSwitchWeaponKeyboard;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (IsFalling())
        {
            characterStateMachine.ChangeState(character.AirBornState);
        }

        if (IsGrounded())
        {
            characterStateMachine.ChangeState(character.IdleState);
        }

        Vector2 moveDir = character.Controller.GetMoveDirection();

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
    private void OnSwitchWeaponKeyboard()
    {
        character.ChangeWeapon(character.Controller.GetNumKeyPressed());
    }

    public bool IsGrounded()
    {
        return character.RB.velocity.y == 0.0f;
    }

    public bool IsFalling()
    {
        return character.RB.velocity.y < -1f;
    }
}
