using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine
{
    public State currentState { get; protected set; }

    public BaseStateMachine(State state)
    {
        triggers = new Dictionary<TransitionTriggerState, TransitionTrigger>();
        TransitionTriggerState[] triggerStates = (TransitionTriggerState[])Enum.GetValues(typeof(TransitionTriggerState));

        for (int i = 0; i < triggerStates.Length; i++)
        {
            triggers.Add(triggerStates[i], new TransitionTrigger()
            {
                isSet = false,
                isTrigger = false
            });
        }
        ChangeState(state);
    }

    public Action<Type> OnStateChange;

    protected Dictionary<TransitionTriggerState, TransitionTrigger> triggers = null;

    public virtual void SetTrigger(TransitionTriggerState trigger)
    {
        if (triggers.ContainsKey(trigger))
        {
            triggers[trigger].isSet = true;
            CheckTransitionStateChange();
            triggers[trigger].isSet = false;
        }
    }

    public virtual bool CheckTriggerSet(TransitionTriggerState trigger)
    {
        if (triggers.ContainsKey(trigger))
            return triggers[trigger].isSet;

        return false;
    }

    protected virtual void ChangeState(State state)
    {
        if (currentState == state)
            return;

        currentState?.OnExitState();
        currentState = state;

        currentState?.OnEnterState();
        OnStateChange?.Invoke(currentState.GetType());
    }

    public void Tick()
    {
        if (!CheckTransitionStateChange())
            currentState?.OnTick();
    }

    protected virtual bool CheckTransitionStateChange()
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
                    ChangeState(currentState.transitions[i].destinationState);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Add source state transition to destination state with conditional
    /// </summary>
    /// <param name="sourceState"></param>
    /// <param name="destinationState"></param>
    /// <param name="conditional"></param>
    public virtual void AddTransitions(State sourceState, State destinationState, Func<bool> conditional)
    {
        if (sourceState.transitions == null)
            sourceState.transitions = new List<Transition>();

        sourceState.transitions.Add(new Transition(destinationState, conditional));
    }

    /// <summary>
    /// Add multiple source states transitions to one Destination state with a single conditional
    /// </summary>
    /// <param name="fromStates"></param>
    /// <param name="destinationState"></param>
    /// <param name="conditional"></param>
    public virtual void AddTransitions(State[] fromStates, State destinationState, Func<bool> conditional)
    {
        for (int i = 0; i < fromStates.Length; i++)
        {
            AddTransitions(fromStates[i], destinationState, conditional);
        }
    }
}

public enum TransitionTriggerState
{
    JumpSetUp,
    BackToIdle,
    ForceInitiated,
    Respawning,
    GameStart,
    GamePause,
    GamePlay,
    GameEnd
}

public class TransitionTrigger
{
    public bool isSet;
    public bool isTrigger;
}