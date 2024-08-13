using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : State
{
    protected BaseCharacter character = null;

    public override void OnEnterState()
    {
        Debug.Log("entered: " + GetType());
    }

    public override void OnExitState()
    {
        Debug.Log("exited: " + GetType());
    }
}
