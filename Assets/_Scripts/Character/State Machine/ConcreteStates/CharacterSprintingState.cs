using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CharacterSprintingState : CharacterState
{
    const string SPEED_PARAM = "Speed";
    const string DIRX_PARAM = "DirX";
    const string DIRY_PARAM = "DirY";

    [System.Serializable]
    public struct Descriptor
    {
        public Transform CameraTransform;
        public float Speed;
        public float MaxSpeed;
    }

    Descriptor _desc;

    public CharacterSprintingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        base.EnterState();
        character.Controller.onInteract += OnInteract;
        character.Controller.onJump += OnJump;
        character.Controller.onSprintExit += OnSprintExit;
        character.Controller.onShootEnter += OnShootEnter;
        character.Controller.onShootExit += OnShootExit;
        character.Controller.onReload += OnReload;
    }

    public override void ExitState()
    {
        base.ExitState();
        character.Controller.onInteract -= OnInteract;
        character.Controller.onJump -= OnJump;
        character.Controller.onSprintExit -= OnSprintExit;
        character.Controller.onShootEnter -= OnShootEnter;
        character.Controller.onShootExit -= OnShootExit;
        character.Controller.onReload -= OnReload;
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
    private void OnSprintExit()
    {
        Vector2 moveDir = character.Controller.GetMoveDirection();
        if (moveDir != Vector2.zero)
        { 
            characterStateMachine.ChangeState(character.MovingState); 
        }
        else
        {
            characterStateMachine.ChangeState(character.IdleState);
        }
    }

    private void OnShootEnter()
    {
        character.Weapon._isShooting = true;
        if (character.Weapon.StateMachine.CurrentWeaponState != character.Weapon.ReloadingState)
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
