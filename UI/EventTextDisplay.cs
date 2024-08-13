using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventTextDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI eventText = null;

    private void Start()
    {
        GameManager.GameFinished += OnGameFinish;     
    }

    void OnGameStart()
    {
        eventText.SetText("START");
        eventText.enabled = true;
    }

    void OnGameFinish()
    {
        eventText.SetText("FINISH");
        eventText.enabled = true;
    }
}
