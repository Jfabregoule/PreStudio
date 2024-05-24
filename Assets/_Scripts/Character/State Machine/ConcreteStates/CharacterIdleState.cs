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
    }

    public override void ExitState()
    {
        base.ExitState();

        character.Controller.onInteract -= OnInteract;
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
}
