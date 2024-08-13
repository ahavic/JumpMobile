using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    BaseStateMachine gameStateMachine = null;
    public BaseStateMachine GameStateMachine { get => gameStateMachine; protected set => gameStateMachine = value; }

    public BaseCharacter playerCharacter = null;
    [SerializeField] Transform initialSpawnPoint = null;

    public static CameraController CameraController { get; protected set; }

    static ScoreSystem scoreSystem = null;
    public static ScoreSystem ScoreSystem { get => scoreSystem; protected set => scoreSystem = value; }

    static Action gameStarted;
    public static event Action GameStarted
    {
        add
        {
            if (gameStarted == null || !gameStarted.GetInvocationList().Contains(value))
            {
                gameStarted += value;
            }
        }
        remove
        {
            if (gameStarted != null && gameStarted.GetInvocationList().Contains(value))
            {
                gameStarted -= value;
            }
        }
    }

    static Action gameFinished;
    public static event Action GameFinished
    {
        add
        {
            if(gameFinished == null || !gameFinished.GetInvocationList().Contains(value))
            {
                gameFinished += value;
            }
        }
        remove
        {
            if (gameFinished != null && gameFinished.GetInvocationList().Contains(value))
            {
                gameFinished -= value;
            }
        }
    }

    public static bool isGameEnded { get; protected set; }

    private void Awake()
    {    
        Instance = this;

        GameStartState gameStartState = new GameStartState();
        GameplayState gameplayState = new GameplayState();
        GameEndState gameEndState = new GameEndState();
        PauseState pauseState = new PauseState();

        gameStateMachine = new BaseStateMachine(gameStartState);

        gameStateMachine.AddTransitions(gameStartState, gameplayState, () => gameStateMachine.CheckTriggerSet(TransitionTriggerState.GamePlay));
        gameStateMachine.AddTransitions(gameplayState, gameEndState, () => gameStateMachine.CheckTriggerSet(TransitionTriggerState.GameEnd));
        gameStateMachine.AddTransitions(gameplayState, pauseState, () => gameStateMachine.CheckTriggerSet(TransitionTriggerState.GamePause));
        gameStateMachine.AddTransitions(pauseState, gameplayState, () => gameStateMachine.CheckTriggerSet(TransitionTriggerState.GamePlay));
    }

    private void OnEnable()
    {
        CameraController = FindObjectOfType<CameraController>();
        if(FindObjectOfType<BaseCharacter>() == null)        
            playerCharacter = Instantiate(playerCharacter, initialSpawnPoint.position, Quaternion.identity);        

        scoreSystem = GetComponentInChildren<ScoreSystem>();
    }

    public static void GameStart()
    {
        gameStarted();
    }

    public static void GameFinish(BaseCharacter character)
    {
        gameFinished();
        isGameEnded = true;
    }

    public static void GameStateTrigger(TransitionTriggerState trigger)
    {
        Instance.gameStateMachine.SetTrigger(trigger);
    }
}
