using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlideState : CharacterState
{
    public bool frameDelay { get; protected set; }
    float totalDelayTime = .05f;
    float delayTime = .05f;

    public GroundSlideState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        character.Anim.Play("GroundSlide");

        frameDelay = true;
        delayTime = totalDelayTime;

        base.OnEnterState();
    }


    public override void OnTick()
    {
        if (frameDelay)
        {
            delayTime -= Time.deltaTime;
            if (delayTime < 0)
            {
                frameDelay = false;
            }
        }
    }

    public override void OnExitState()
    {
        frameDelay = false;
        base.OnExitState();
    }
}
