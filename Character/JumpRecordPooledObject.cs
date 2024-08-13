using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct JumpData
{
    public float forceData;
    public float angleData;
}

public class JumpRecordPooledObject : MonoBehaviour, IPoolObject
{
    Transform thisTransform = null;
    [SerializeField] LineRenderer line = null;
    [SerializeField] TextMeshProUGUI forceText = null;
    [SerializeField] TextMeshProUGUI angleText = null;

    private void Awake()
    {
        thisTransform = transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Initialize(Vector3 initPos)
    {
        thisTransform.position = new Vector3(initPos.x, initPos.y, -8f);
        line.SetPosition(0, thisTransform.position);
    }

    public void SetRecordData(JumpData data, Vector3[] linePoints)
    {
        line.SetPositions(linePoints);        
        forceText.SetText(data.forceData.ToString("0.0"));
        angleText.SetText(data.angleData.ToString("0.0"));
    }
}
