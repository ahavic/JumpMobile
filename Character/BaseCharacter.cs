using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCharacter : MonoBehaviour
{
    protected Transform thisTransform = null;
    [SerializeField] Rigidbody rigidBody = null;
    public Rigidbody RigidBody { get => rigidBody; }

    public Vector3 lastVelocity { get; protected set; }

    [SerializeField] protected CharacterData data = null;
    public CharacterData Data { get => data; }

    [SerializeField] Animator anim = null;
    public Animator Anim { get => anim; }
    
    public CharacterStateMachine stateMachine { get; protected set; }

    [NonSerialized] public Vector3 forceDirectionVector = new Vector3(0, 1, 0);
    public LineRenderer forceDirectionLine = null;

    ResetPosition resetPosition = null;

    public int boosterAmount = 2;

    CharacterPlatformCheck charPlatCheck = null; 

    #region On button press inputs
    public Action OnAction01DownInput = null;
    public Action OnAction02Input = null;
    public Action OnCancelDownInput = null;
    public Action OnRInput = null;
    public Action OnBInput = null;
    public Action OnCameraToggleInput = null;
    #endregion

    public float movementAxis { get; protected set; }


    private void Awake()
    {
        thisTransform = transform;
        charPlatCheck = GetComponent<CharacterPlatformCheck>();

        CharacterState idleState = new IdleState(this);
        CharacterState directionState = new SetDirectionAndForceState(this);
        JumpingState jumpState = new JumpingState(this);
        CharacterState respawningState = new RespawningState(this);
        CharacterState freeFallState = new FreeFallState(this);
        GroundSlideState groundSlideState = new GroundSlideState(this);
        
        stateMachine = new CharacterStateMachine(idleState);

        stateMachine.AddTransitions(idleState, directionState, () => stateMachine.CheckTriggerSet(TransitionTriggerState.JumpSetUp));
        stateMachine.AddTransitions(directionState, jumpState, () => stateMachine.CheckTriggerSet(TransitionTriggerState.ForceInitiated));
        stateMachine.AddTransitions(directionState, freeFallState, () => !charPlatCheck.isGrounded);
        stateMachine.AddTransitions(jumpState, groundSlideState, () => charPlatCheck.isGrounded && !jumpState.frameDelay);
        stateMachine.AddTransitions(groundSlideState, freeFallState, () => !charPlatCheck.isGrounded);
        stateMachine.AddTransitions(groundSlideState, idleState, () => rigidBody.velocity == Vector3.zero && charPlatCheck.isGrounded);
        stateMachine.AddTransitions(groundSlideState, directionState, () => !groundSlideState.frameDelay);
        //stateMachine.AddTransitions(groundSlideState, directionState, () => !groundSlideState.frameDelay);
        stateMachine.AddTransitions(new State[] { freeFallState, jumpState}, idleState, () => rigidBody.velocity == Vector3.zero && !jumpState.frameDelay && charPlatCheck.isGrounded);
        stateMachine.AddTransitions(new State[] { jumpState, idleState, freeFallState, respawningState }, respawningState, () => stateMachine.CheckTriggerSet(TransitionTriggerState.Respawning));
        stateMachine.AddTransitions(respawningState, idleState, () => rigidBody.velocity == Vector3.zero && charPlatCheck.isGrounded);
        stateMachine.AddTransitions(new State[] { directionState }, idleState, () => stateMachine.CheckTriggerSet(TransitionTriggerState.BackToIdle) && charPlatCheck.isGrounded);
        stateMachine.AddTransitions(idleState, freeFallState, () => rigidBody.velocity != Vector3.zero && !charPlatCheck.isGrounded);
        stateMachine.AddTransitions(freeFallState, groundSlideState, () => charPlatCheck.isGrounded);
    }

    private void Start()
    {
        resetPosition = FindObjectOfType<ResetPosition>();
        resetPosition.SetResetPosition(thisTransform.position, false);
        UpdateBoosterAmountUI(boosterAmount);       
    }

    private void Update()
    {
        stateMachine?.Tick();
        BoundsCheck();
        lastVelocity = rigidBody.velocity;

        if (rigidBody.velocity.magnitude > data.maxVelocity)
            rigidBody.velocity = rigidBody.velocity.normalized * data.maxVelocity;
    }

    public void BoundsCheck()
    {
        if (thisTransform.position.y < resetPosition.yResetBound)        
            stateMachine.SetTrigger(TransitionTriggerState.Respawning);           
    }

    public void SetMovementAxis(float axis)
    {
        movementAxis = axis;
    }

    public void SetTrigger(TransitionTriggerState trigger)
    {
        stateMachine.SetTrigger(trigger);
    }

    public void RecordJumpData(JumpData data)
    {
        JumpRecordPool.Instance.DisablePooledObjects(typeof(JumpRecordPooledObject));
        JumpRecordPooledObject record = (JumpRecordPooledObject)JumpRecordPool.Instance.GetFromPool(typeof(JumpRecordPooledObject));
        record.Initialize(thisTransform.position);

        Vector3[] positions = new Vector3[forceDirectionLine.positionCount];
        forceDirectionLine.GetPositions(positions);

        record.SetRecordData(data, positions);
        record.GetGameObject().SetActive(true);
    }

    public void UpdateBoosterAmountUI(int amount)
    {
        PlayerUI.UpdateBoosterAmount(amount);
    }

    public void SetResetPosition()
    {
        if(stateMachine.currentState.GetType() == typeof(IdleState) && charPlatCheck.CheckRespawnPlatform())
            resetPosition.SetResetPosition(thisTransform.position, true);
    }

    public void ReachedEndPlatform()
    {
        if(stateMachine.currentState is IdleState)
        {
            GameManager.GameFinish(this);
        }
    }    
}
