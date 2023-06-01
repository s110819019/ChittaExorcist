using ChittaExorcist.CharacterCore;
using ChittaExorcist.GameCore.DialogueSettings;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class NPC : MonoBehaviour
    {
            #region w/ NPC Data

    [SerializeField] private NPCDataSO npcData;

    public NPCDataSO NPCData => npcData;

    [SerializeField] public Transform patrolPoint1;
    [SerializeField] public Transform patrolPoint2;
    
    #endregion
    
    #region w/ Event Subscribe

    protected virtual void SetSubscribeEvents()
    {
        DialogueTrigger.OnDialogueEnter += OnDialogueEnter;
        DialogueTrigger.OnDialogueEnterCheckPlayerTransform += OnDialogueEnterCheckPlayerTransform;
    }

    protected virtual void SetUnsubscribeEvents()
    {
        DialogueTrigger.OnDialogueEnter -= OnDialogueEnter;
        DialogueTrigger.OnDialogueEnterCheckPlayerTransform -= OnDialogueEnterCheckPlayerTransform;
    }
    
    #endregion

    #region w/ Dialogue Trigger

    protected virtual void OnDialogueEnter() { }

    protected virtual void OnDialogueEnterCheckPlayerTransform(Transform targetTransform) { }
    
    #endregion

    #region w/ Components

    // public SpriteRenderer SpriteRenderer { get; private set; }
    public Core Core { get; private set; }
    public Animator Animator { get; private set; }
    public NPCAnimationEventHandler EventHandler  { get; private set; }
    // public ObjectPool ObjectPool { get; private set; }
    public DialogueTrigger DialogueTrigger { get; private set; }
    
    #endregion
    
    #region w/ State Variables

    public NPCStateMachine StateMachine { get; private set; }
    protected virtual void InitializeStates() { }

    #endregion
    
    #region w/ Core Components
    
    // Movement
    protected Movement Movement => _movement ? _movement : Core.GetCoreComponent(out _movement);
    private Movement _movement;

    #endregion
    
    #region w/ Unity Callback Functions

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();
        Animator = GetComponent<Animator>();

        StateMachine = new NPCStateMachine();
        
        // ObjectPool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();

        EventHandler = GetComponent<NPCAnimationEventHandler>();

        DialogueTrigger = GetComponentInChildren<DialogueTrigger>();
        
        InitializeStates(); // 初始化 state
    }

    protected virtual void Start()
    {
    
    }

    protected virtual void Update()
    {
        Core.LogicUpdate(); 
        StateMachine.CurrentState.LogicUpdate();
        Animator.SetFloat("yVelocity", Movement.Rigidbody2D.velocity.y);
        // 偵錯用
        // Debug.Log(StateMachine.currentState);
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    protected virtual void OnEnable()
    {
        SetSubscribeEvents();
    }

    protected void OnDisable()
    {
        SetUnsubscribeEvents();
    }

    #endregion
    }
}