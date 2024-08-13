using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStateMachine : BaseStateMachine
{
    public CharacterStateMachine(CharacterState state) : base(state) 
    {
        
    }   

    protected override void ChangeState(State state)
    {
        if (currentState == state)
            return;

        currentState?.OnExitState();
        currentState = state;

        currentState?.OnEnterState();
        OnStateChange?.Invoke(currentState.GetType());
    }

    protected override bool CheckTransitionStateChange()
    {
        if (currentState != null)
        {
            if (currentState.transitions == null)
            {
                Debug.LogWarning("There are no transitions in current state: " + currentState.GetType());
                return false;
            }

            for (int i = 0; i < currentState.transitions.Count; i++)
            {
                if (currentState.transitions[i].condition())
                {
                    ChangeState(currentState.transitions[i].destinationState as CharacterState);
                    return true;
                }
            }
        }

        return false;
    }

}
