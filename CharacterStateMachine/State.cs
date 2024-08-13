using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public List<Transition> transitions = null;
    public virtual void OnEnterState() { }
    public virtual void OnExitState() { }

    public virtual void OnTick() { }
}
