using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallState : CharacterState
{
    public FreeFallState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        character.Anim.Play("Jump");
    }
}
