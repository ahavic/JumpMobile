using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ScreenInputHandler : InputHandler
{
    Dictionary<Type, GameObject[]> enabledUIObjsDict = null;

    [SerializeField] GameObject initiateJumpButton = null;
    [SerializeField] GameObject cancelJumpButton = null;
    [SerializeField] GameObject setRespawnPointButton = null;
    [SerializeField] GameObject useBoosterButton = null;
    [SerializeField] GameObject toggleCameraViewButton = null;
    [SerializeField] GameObject forceText = null;
    [SerializeField] GameObject angleText = null;

    List<GameObject> allUIObjsList = null;

    private void Awake()
    {
        enabledUIObjsDict = new Dictionary<Type, GameObject[]>();

        enabledUIObjsDict.Add(typeof(PauseState), new GameObject[] { });

        enabledUIObjsDict.Add(typeof(IdleState), new GameObject[] { initiateJumpButton, setRespawnPointButton, toggleCameraViewButton });
        enabledUIObjsDict.Add(typeof(SetDirectionAndForceState), new GameObject[] { cancelJumpButton, forceText, angleText });
        enabledUIObjsDict.Add(typeof(JumpingState), new GameObject[] { useBoosterButton });

        allUIObjsList = new List<GameObject>();
        foreach(var kvp in enabledUIObjsDict)
        {
            for(int i = 0; i < enabledUIObjsDict[kvp.Key].Length; i++)
            {
                if(!allUIObjsList.Contains(enabledUIObjsDict[kvp.Key][i]))                
                    allUIObjsList.Add(enabledUIObjsDict[kvp.Key][i]);                
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        character.stateMachine.OnStateChange += EnableStateButtons;
        GameManager.Instance.GameStateMachine.OnStateChange += EnableStateButtons;
        int x = 1;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        character.stateMachine.OnStateChange -= EnableStateButtons;
        GameManager.Instance.GameStateMachine.OnStateChange -= EnableStateButtons;
    }

    #region Button Press callbacks
    public void OnInitiateJumpButtonPressed()
    {
        character.OnAction01DownInput?.Invoke();
    }

    public void OnToggleCameraSizeButtonPress()
    {
        character.OnCameraToggleInput?.Invoke();
    }

    public void OnCancelButtonPressed()
    {
        character.OnCancelDownInput?.Invoke();
    }

    public void OnSetRespawnButtonPressed()
    {
        character.OnRInput?.Invoke();
    }

    public void OnBoosterButtonPressed()
    {
        character.OnBInput?.Invoke();
    }
    #endregion

    public void EnableStateButtons(Type state)
    {
        for(int i = 0; i < allUIObjsList.Count; i++)
        {
            allUIObjsList[i].gameObject.SetActive(false);
        }

        if(!(GameManager.Instance.GameStateMachine.currentState is GameplayState))
        {
            state = typeof(PauseState);
        }
        else
        {
            state = character.stateMachine.currentState.GetType();
        }
            

        if(enabledUIObjsDict.ContainsKey(state))
        {
            for(int i = 0; i < enabledUIObjsDict[state].Length; i++)
            {
                enabledUIObjsDict[state][i].gameObject.SetActive(true);
            }
        }
    }

    protected override void Update()
    {
        //Leave empty for now
    }
}