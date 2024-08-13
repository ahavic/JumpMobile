using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StateUIData", menuName = "ScriptableObjects/UI/State UI Data")]
[System.Serializable]
public class StateUIData : ScriptableObject
{
    [SerializeField] System.Type t;
}
