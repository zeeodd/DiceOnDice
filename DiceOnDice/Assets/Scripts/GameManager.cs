using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Testing Debug")]
    public bool debugOn;
    public Camera sceneCamera;
    public Canvas sceneCanvas;

    public FiniteStateMachine<GameManager> _GameManagerStateMachine;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _InitializeServices();
        _InitializeClasses();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _InitializeFSM();
        _RegisterListeners();

        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(3000, 1000);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Reset();
    }

    private void OnDestroy()
    {
        _UnregisterListeners();
    }
    #endregion

    #region Class Functions
    private void _InitializeServices()
    {
        Services.GameManager = this;
        Services.AudioManager = new AudioManager();
        Services.EventManager = new EventManager();
    }

    private void _InitializeClasses()
    {
        Logger.DEBUG_ON = debugOn;
        Services.AudioManager.Initialize();
    }

    private void _InitializeFSM()
    {
        _GameManagerStateMachine = new FiniteStateMachine<GameManager>(this);
        _GameManagerStateMachine.TransitionTo<Introduction>();
    }

    public void Reset()
    {
        Globals.muted = false;

        Services.AudioManager.Reset();

        SceneManager.LoadScene("SampleScene");
    }
    #endregion

    #region Event Listeners
    private void _RegisterListeners()
    {
        
    }

    private void _UnregisterListeners()
    {
        
    }
    #endregion

    #region States
    /*
    A template GameState for high level navigations such as from
    the Main Menu to the Cafe or to settings
    */
    public class GameState : FiniteStateMachine<GameManager>.State
    {
        public override void OnEnter() { }
        public override void Update() { }
        public override void OnExit() { }
    }

    private class Introduction : GameState
    {
        public override void OnEnter()
        {
            Logger.Log("Entering Introduction");
        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            
        }
    }
    #endregion
}
