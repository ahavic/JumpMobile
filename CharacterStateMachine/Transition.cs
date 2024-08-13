using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition
{
    public State destinationState { get; protected set; }
    public Func<bool> condition = null;

    public Transition(State destinationState, Func<bool> condition)
    {
        this.destinationState = destinationState;
        this.condition = condition;
    }
}
