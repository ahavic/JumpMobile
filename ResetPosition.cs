using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    static Transform thisTransform = null;

    int resetsRemaining = 3;
    public int ResetsRemaining { get => resetsRemaining; protected set => resetsRemaining = value; }

    public float yResetBound = -10f;
    public Vector3 resetPosition { get; protected set; }

    private void Awake()
    {
        thisTransform = transform;
    }

    public void AddResets(int amount)
    {
        resetsRemaining += amount;
    }

    public void SetResetPosition(Vector3 pos, bool isPlayerSet)
    {
        if(isPlayerSet)
        {
            if(ResetsRemaining >= 0 && thisTransform.position != pos)
            {
                --resetsRemaining;
                resetPosition = pos;
            }    
        }
        else
            resetPosition = pos + Vector3.up;

        thisTransform.position = resetPosition;
    }   

    public static void TriggerResetPosition(Transform target)
    {
        target.position = thisTransform.position;
    }
}
