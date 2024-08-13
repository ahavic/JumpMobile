using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningState : CharacterState
{
    public RespawningState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        character.RigidBody.velocity = Vector3.zero;
        ResetPosition.TriggerResetPosition(character.transform);
    }

}
