using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText = null;

    private float runningScore;

    int maxBusterUsage = 10;
    int bustersUsage = 10;

    int maxResetPositionUsage = 10;
    int resetPositionUsage = 10;

    public float RunningScore
    {
        get { return runningScore; }
        protected set { runningScore = value; }
    }

    private void Awake()
    {
        //ScoreText.SetText("Score: " + runningScore);
    }

    private void Start()
    {
        GameManager.GameStarted += OnGameStarted;
        GameManager.GameFinished += OnGameFinish;
    }

    private void OnDestroy()
    {
        GameManager.GameStarted -= OnGameStarted;
        GameManager.GameFinished -= OnGameFinish;
    }

    void OnGameStarted()
    {
        InitializeScore(0);
    }

    public void InitializeScore(float time)
    {
        runningScore = 0;
        bustersUsage = maxBusterUsage;
        resetPositionUsage = maxResetPositionUsage;
    }

    public void BusterUsed()
    {
        if(bustersUsage > 0)
            bustersUsage--;        
    }

    public void ResetPositionUsed()
    {
        if (resetPositionUsage > 0)
            resetPositionUsage--;
    }

    public void OnGameFinish()
    {
        runningScore += TimeManager.gameTime;
        BusterScoreMod();
        ResetPositionScoreMod();
        ScoreText.SetText("Score: " + runningScore.ToString("0.0"));
        ScoreText.enabled = true;
    }

    void BusterScoreMod()
    {
        float busterMod = Utils.Remap(0, maxBusterUsage, .7f, 1, bustersUsage);
        RunningScore *= busterMod;
    }

    void ResetPositionScoreMod()
    {
        float resetPositionMod = Utils.Remap(0, maxResetPositionUsage, .5f, 1, resetPositionUsage);
        runningScore *= resetPositionMod;
    }
}
