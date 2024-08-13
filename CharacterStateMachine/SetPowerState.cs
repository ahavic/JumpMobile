using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPowerState : CharacterState
{
    float power = 1f;
    Vector3 powerVector = Vector3.zero;
    float powerLineMod = 3f;

    public SetPowerState(BaseCharacter character)
    {
        this.character = character;               
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        this.character.OnAction01DownInput += InitiateForce;
        this.character.OnAction02Input += PowerUp;

        power = 1f;
        character.forceDirectionLine.startColor = Color.yellow;
        character.forceDirectionLine.endColor = Color.red;
        powerVector = character.forceDirectionVector * power;
        character.forceDirectionLine.SetPosition(1, powerVector/powerLineMod + character.transform.position);
    }

    public override void OnTick()
    {
        power -= Time.deltaTime / 1.5f;
        power = Mathf.Clamp(power, character.Data.minPower, character.Data.maxPower);
        powerVector = character.forceDirectionVector * power;
        character.forceDirectionLine.SetPosition(1, powerVector / powerLineMod + character.transform.position);
    }

    void PowerUp()
    {
        power += Time.deltaTime * 4.5f;
        power = Mathf.Clamp(power, character.Data.minPower, character.Data.maxPower);
    }

    private void InitiateForce()
    {
        character.RigidBody.AddForce(character.forceDirectionVector * power, ForceMode.Impulse);
        character.SetTrigger(TransitionTriggerState.ForceInitiated);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        this.character.OnAction01DownInput -= InitiateForce;
        this.character.OnAction02Input -= PowerUp;
        character.forceDirectionLine.enabled = false;
    }
}
