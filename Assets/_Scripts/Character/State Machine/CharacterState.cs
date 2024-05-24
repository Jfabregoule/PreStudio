using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    protected Character character;
    protected CharacterStateMachine characterStateMachine;

    public CharacterState(Character character, CharacterStateMachine characterStateMachine)
    {
        this.character = character;
        this.characterStateMachine = characterStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
