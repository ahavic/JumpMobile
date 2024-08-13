using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharacterState
{
    public IdleState(BaseCharacter character)
    {
        this.character = character;
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        character.forceDirectionLine.enabled = false;
        character.OnCameraToggleInput += ToggleCameraSize;
        character.OnRInput += OnSetResetPosition;
        character.OnAction01DownInput += BeginJumpSetUp;
        character.Anim.Play("Idle");
    }

    public override void OnExitState()
    {
        base.OnExitState();
        character.OnCameraToggleInput -= ToggleCameraSize;
        character.OnAction01DownInput -= BeginJumpSetUp;
        character.OnRInput -= OnSetResetPosition;
    }

    void ToggleCameraSize()
    {
        GameManager.CameraController.ToggleCamera();
    }

    void OnSetResetPosition()
    {
        character.SetResetPosition();        
    }

    public void BeginJumpSetUp()
    {
        character.SetTrigger(TransitionTriggerState.JumpSetUp);
    }
}
