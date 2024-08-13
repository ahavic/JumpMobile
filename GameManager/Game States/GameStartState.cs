using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartState : State
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        TimerCoroutineManager.SetUpTimer(StartGame, 2.5f);
    }

    void StartGame()
    {
        GameManager.GameStart();
        GameManager.GameStateTrigger(TransitionTriggerState.GamePlay);
    }
}
