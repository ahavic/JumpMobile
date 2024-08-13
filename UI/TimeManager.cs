using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;


public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance = null;

    [SerializeField] TextMeshProUGUI timeTMP = null;

    bool isInGamePlay = false;

    public static float gameTime { get; protected set; }

    private void Awake()
    {
        Instance = this;
        timeTMP.SetText("00:00");
        gameTime = 0f;
        SetTimeText(gameTime);
    }

    private void Start()
    {
        GameManager.GameStarted += OnGameStarted;
        GameManager.GameFinished += OnGameEnded;
    }

    private void OnDestroy()
    {
        GameManager.GameStarted -= OnGameStarted;
        GameManager.GameFinished += OnGameEnded;
    }

    void Update()
    {
        if (isInGamePlay)
        {
            gameTime += Time.deltaTime;
            SetTimeText(gameTime);
        }
    }

    void SetTimeText(float time)
    {
        int seconds = (int)(gameTime % 60f);
        int minutes = (int)(gameTime / 60f);
        float mil = time;
        mil.ToString("F1");        

        string timeText = TimeSpan.FromSeconds(time).ToString("mm':'ss'.'ff");
        timeTMP.SetText(timeText);
    }

    void OnGameStarted()
    {
        isInGamePlay = true;
    }

    void OnGameEnded()
    {
        isInGamePlay = false;
    }
}
