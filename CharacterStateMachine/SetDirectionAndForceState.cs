using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SetDirectionAndForceState : CharacterState
{
    bool isCurrentlyPressed = false;
    
    float power = 1f;
    JumpData jumpData;

    Vector3 forceVector = Vector3.zero;
    Vector3 startPosition = Vector2.zero;
    Vector3 endPosition = Vector2.zero;
    const float powerlineModMax = 80f;

    const float zOffset = -8f; 
    
    public SetDirectionAndForceState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();

        isCurrentlyPressed = false;

        character.OnAction01DownInput += InitiateForce;
        character.OnCancelDownInput += CancelJump;

        forceVector = Vector3.zero;
        startPosition = Vector3.zero;
        endPosition = Vector3.zero;

        power = 0f;
        character.forceDirectionLine.startColor = Color.yellow;
        character.forceDirectionLine.endColor = Color.red;
        character.Anim.Play("SetJump");
    }

    public override void OnTick()
    {
        SetForceVector();
        ReadCurrentJumpData();
    }

    void SetForceVector()
    {
        power = Utils.Remap(0f, powerlineModMax, 0f, character.Data.maxPower, forceVector.sqrMagnitude);

        if (!isCurrentlyPressed && power < character.Data.minPower)
        {
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            power = 0;
            forceVector = Vector3.zero;
            character.forceDirectionLine.enabled = false;
            isCurrentlyPressed = Touchscreen.current.primaryTouch.press.isPressed;
            if (!isCurrentlyPressed)
                return;

            startPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.startPosition.ReadValue());
            startPosition.z = zOffset;
            endPosition = startPosition;
            character.forceDirectionLine.SetPosition(0, startPosition);
        }
        else if (!isCurrentlyPressed && power > character.Data.minPower)
        {            
            forceVector.Normalize();
            InitiateForce();
            character.forceDirectionLine.enabled = false;
            return;
        }
        startPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.startPosition.ReadValue());
        startPosition.z = zOffset;

        character.forceDirectionLine.enabled = true;
        endPosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
        endPosition.z = zOffset;
        character.forceDirectionLine.SetPositions(new Vector3[] { startPosition, endPosition });
        forceVector = startPosition - endPosition;
        forceVector.z = 0;       

        isCurrentlyPressed = Touchscreen.current.primaryTouch.press.isPressed;
    }

    private void ReadCurrentJumpData()
    {
        jumpData.forceData = power < character.Data.minPower? 0f : power;
        jumpData.angleData = power <= 0 ? 0 : Vector2.Angle(Vector2.right, forceVector);
        PlayerUI.UpdateCurrentJumpData(jumpData);
    }

    private void InitiateForce()
    {
        character.RigidBody.AddForce(forceVector * power, ForceMode.Impulse);
        character.forceDirectionLine.SetPositions(new Vector3[] { character.transform.position.AxisMod(z: zOffset), character.transform.position.AxisMod(z: zOffset) + forceVector * power/3.5f });
        JumpData data = new JumpData()
        {
            forceData = power,
            angleData = Vector2.Angle(Vector2.right, forceVector)
        };
        
        character.RecordJumpData(data);
        character.SetTrigger(TransitionTriggerState.ForceInitiated);
    }

    private void CancelJump()
    {
        character.SetTrigger(TransitionTriggerState.BackToIdle);
    }

    public override void OnExitState()
    {
        character.OnAction01DownInput -= InitiateForce;
        character.OnCancelDownInput -= CancelJump;

        character.forceDirectionVector = character.forceDirectionVector.normalized;
        character.forceDirectionLine.enabled = false;
    }
}
