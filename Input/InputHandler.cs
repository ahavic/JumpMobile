using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public bool isGameplayControlsEnabled { get; protected set; }
    protected BaseCharacter character = null;
    [SerializeField] protected InputData inputData = null;

    protected virtual void Start()
    {
        isGameplayControlsEnabled = false;        
        character = GetComponent<BaseCharacter>();

        if (character == null)
            character = FindObjectOfType<BaseCharacter>();

        GameManager.GameStarted += EnableGamePlayControls;
        GameManager.GameFinished += DisableGameplayControls;
    }

    protected virtual void OnDestroy()
    {
        GameManager.GameStarted -= EnableGamePlayControls;
        GameManager.GameFinished -= DisableGameplayControls;
    }

    protected virtual void Update()
    {
        if (isGameplayControlsEnabled)
        {
            ReadInputs();
            MovementInput();
        }
    }

    protected virtual void ReadInputs()
    {
        if (Input.GetKeyDown(inputData.inputDict[InputType.OnAction01Down].inputKey))
            character.OnAction01DownInput?.Invoke();
        if (Input.GetKeyDown(inputData.inputDict[InputType.OnCancelDown].inputKey))
            character.OnCancelDownInput?.Invoke();
        if (Input.GetKey(inputData.inputDict[InputType.OnAction02].inputKey))
            character.OnAction02Input?.Invoke();
        if (Input.GetKeyDown(inputData.inputDict[InputType.OnRespawnDown].inputKey))
            character.OnRInput?.Invoke();
        if (Input.GetKeyDown(inputData.inputDict[InputType.OnBoosterDown].inputKey))
            character.OnBInput?.Invoke();
    }

    void MovementInput()
    {
        character.SetMovementAxis(Input.GetAxisRaw("Horizontal"));
    }

    public virtual void EnableGamePlayControls()
    {
        isGameplayControlsEnabled = true;
    }

    public virtual void DisableGameplayControls()
    {
        isGameplayControlsEnabled = false;
    }
}
