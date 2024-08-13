using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : Platform
{
    [SerializeField] float speed = 1;


    private void Start()
    {
        TimerCoroutineManager.SetUpTimer(SwitchPlatformOnOff, 0f, 15f, delegate { return true; }, 0);
    }

    void SwitchPlatformOnOff(float time, float firstBound, float secondBound)
    {
        int v = (int)(firstBound + Mathf.PingPong(time * speed, secondBound));
        if( v <= firstBound)
        {
            gameObject.SetActive(false);
        }
        else if( v >= secondBound-5)
        {
            gameObject.SetActive(true);
        }

    }
}
