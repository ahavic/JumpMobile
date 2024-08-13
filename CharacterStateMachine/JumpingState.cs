using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : CharacterState
{
    public bool frameDelay { get; protected set; }
    float totalDelayTime = .1f;
    float delayTime = .08f;

    float boostForce = 9f;

    public JumpingState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        character.OnBInput += OnBusterExplosion;
        frameDelay = true;
        delayTime = totalDelayTime;
        character.Anim.Play("Jump");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        frameDelay = false;
        character.OnBInput -= OnBusterExplosion;
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

    void OnBusterExplosion()
    {
        if(character.boosterAmount > 0 && !frameDelay)
        {
            character.RigidBody.AddForce(Vector2.up * boostForce, ForceMode.Impulse);
            character.boosterAmount--;
            character.UpdateBoosterAmountUI(character.boosterAmount);
            GameManager.ScoreSystem.BusterUsed();
        }
    }
}
