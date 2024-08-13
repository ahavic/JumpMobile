using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType {
    OnAction01Down,
    OnAction02,
    OnBoosterDown,
    OnRespawnDown,
    OnCancelDown,
}

[CreateAssetMenu(fileName = "InputData", menuName = "ScriptableObjects/Inputs/Input Data")]
[System.Serializable]
public class InputData : ScriptableObject
{
    [System.Serializable]
    public struct InputObj
    {
        public InputType inputType;
        public KeyCode inputKey;
        public bool isUnique;
    }

    [SerializeField] InputObj[] inputList = null;
    public Dictionary<InputType, InputObj> inputDict = new Dictionary<InputType, InputObj>();

    public void CheckUniqueKeyBindings()
    {
        HashSet<InputType> objs = new HashSet<InputType>();
        for (int i = 0; i < inputList.Length; i++)
        {
            if (objs.Contains(inputList[i].inputType) && inputList[i].isUnique)
            {
                Debug.LogError("There is more than one input type for the unique type of: " + inputList[i].inputType.ToString());
                break;
            }
            else
            {
                objs.Add(inputList[i].inputType);
            }
        }
    }

    public void RebindInputs()
    {
        inputDict.Clear();
        for(int i = 0; i < inputList.Length; i++)
        {
            if(inputDict.ContainsKey(inputList[i].inputType))
            {
                Debug.LogError("Action " + inputList[i].inputType.ToString() + " is binded more than once");
            }
            else
            {
                inputDict.Add(inputList[i].inputType, inputList[i]);
            }
        }
    }

    private void OnValidate()
    {
        CheckUniqueKeyBindings();
        RebindInputs();
    }
}
