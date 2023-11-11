using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AwakeSolutions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class ConfigData
{
    public float backDuration;
    public float backDelay;

    public float frontDuration;
    public float frontDelay;
}

public class SceneController : MonoBehaviour
{
    private List<AbstractSceneState> _sceneStates;
    private int _currentStateIndex;
    private AbstractSceneState _currentState;
    
    [Header("Media Players")]
    public string VideosFolder;
    [SerializeField] private AwakeMediaPlayer[] _kernStands;
    
    [Header("Layers Alpha")] 
    public AlphaTransition ReadyMessage;

    [Header("Layer Controllers")] 
    public LayerController BackScreenContent;
    public LayerController BackScreenTransition;

    public LayerController FrontScreenWM;
    public LayerController FrontScreenPM;
    public LayerController FrontScreenShowMode;
    public LayerController FrontScreenTransition;

    public float backDuration;
    public float backDelay;

    public float frontDuration;
    public float frontDelay;

    #region private methods

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "config.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ConfigData settingsData = JsonConvert.DeserializeObject<ConfigData>(json);
            
           
            backDuration = settingsData.backDuration;
            backDelay = settingsData.backDelay;

            frontDuration = settingsData.frontDuration;
            frontDelay = settingsData.frontDelay;
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
    }

    private void Start()
    {
        _sceneStates = new List<AbstractSceneState>()
        {
            new WaitingModeState(this),
            new PlayModeState(this),
            new ContentMode(this)
        };

        _currentStateIndex = 0;
        _currentState = _sceneStates[_currentStateIndex];
        _currentState.OnEnter();
    }
    
    private void Update()
    {
        _currentState.OnUpdate();
    }
    
    private void ShowTransitionBack()
    {
        BackScreenTransition.SetOpaque();
        BackScreenTransition.SetSpeed(1);
        BackScreenTransition.Play();
        
        this.DelayAction(backDuration, () => { BackScreenTransition.SetTransparent(); });
    }

    private void CleanMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }
    
    #endregion

    #region public methods
    
    public void SetWaitingMode()
    {
        _currentState.OnExit();
        _currentStateIndex = 0;
        _currentState = _sceneStates[_currentStateIndex];
        _currentState.OnEnter();
    }
    
    public void SetNextState()
    {
        if (_currentStateIndex + 1 < _sceneStates.Count)
        {
            _currentStateIndex++;
            _currentState.OnExit();

            _currentState = _sceneStates[_currentStateIndex];
            _currentState.OnEnter();
        }
        else
        {
            _currentStateIndex = 0;
            _currentState.OnExit();

            _currentState = _sceneStates[_currentStateIndex];
            _currentState.OnEnter();
        }
        
        CleanMemory();
    }

    public void SetKernState(int kernId, bool state)
    {
        string filename = state ? "KernGreen" : "KernRed";
        _kernStands[kernId].Open(VideosFolder, filename, true, true);
    }

    public void ShowReadyMessage()
    {
        ReadyMessage.StartFadeIn();
    }

    public void HideReadyMessage()
    {
        ReadyMessage.StartFadeOut();
    }

    public void TransitionedNextState()
    {
        ShowTransitionBack();
        ShowTransitionFront();
    }
    
    public void ShowTransitionFront()
    {
        FrontScreenTransition.SetOpaque();
        FrontScreenTransition.SetSpeed(1);
        FrontScreenTransition.Play();
        
        this.DelayAction(frontDuration, () => { FrontScreenTransition.SetTransparent(); });
    }

    #endregion
    
}
