using System;
using System.Collections.Generic;
using AwakeSolutions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class WaitingModeState : AbstractSceneState
{
    public WaitingModeState(SceneController controller) : base(controller){}

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Q))
        {
            _controller.ShowTransitionFront();
            _controller.SetNextState();
            AwakeSoundManager.Play("button");
        }
    }

    public override void OnEnter()
    {
        _controller.DelayAction(_controller.backDelay, () =>
        {
            _controller.BackScreenContent.Open(_controller.VideosFolder, "BackScreenBackground", true, true);
        });
        
        _controller.DelayAction(_controller.frontDelay, () => {
            _controller.FrontScreenShowMode.SetTransparent();
            _controller.FrontScreenWM.Open(_controller.VideosFolder, "FrontScreenContentWM", true, true);
            _controller.FrontScreenWM.Play();
            _controller.FrontScreenWM.SetOpaque();});
        
        _controller.PlayBackgroundSound();
    }
}

public class PlayModeState : AbstractSceneState
{
    public PlayModeState(SceneController controller) : base(controller){}
    
    private List<bool> _kernStates = new List<bool>(){false, false, false};
    private UnityEvent<int> _kernDownEvent = new UnityEvent<int>();
    private UnityEvent<int> _kernUpEvent = new UnityEvent<int>();
    private bool _readyMessageShown;
    private bool _isTransitionStarted;

    private void OnKernDown(int kernIndex)
    {
        _controller.SetKernState(kernIndex, true);
        _kernStates[kernIndex] = true;
        AwakeSoundManager.Play("stand");
    }

    private void OnKernUp(int kernIndex)
    {
        _controller.SetKernState(kernIndex, false);
        _kernStates[kernIndex] = false;
    }
    
    public override void OnEnter()
    {
        _controller.DelayAction(_controller.frontDelay, () =>
        {
            _controller.FrontScreenWM.SetTransparent();
            _controller.FrontScreenPM.SetOpaque();
            _controller.FrontScreenPM.Play();
        });
        
        _kernDownEvent.AddListener(OnKernDown);
        _kernUpEvent.AddListener(OnKernUp);

        _readyMessageShown = false;
        _isTransitionStarted = false;
        
        _kernUpEvent.Invoke(0);
        _kernUpEvent.Invoke(1);
        _kernUpEvent.Invoke(2);
    }
    
    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.W)) _kernDownEvent.Invoke(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.E)) _kernDownEvent.Invoke(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.R)) _kernDownEvent.Invoke(2);
        
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.W)) _kernUpEvent.Invoke(0);
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.E)) _kernUpEvent.Invoke(1);
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.R)) _kernUpEvent.Invoke(2);

        if (!_kernStates.Contains(false) && !_readyMessageShown)
        {
            _controller.ShowReadyMessage();
            _readyMessageShown = true;
            AwakeSoundManager.Play("ready");
            if(_controller.FrontScreenPM.ContentPlayer.isPlaying) _controller.FrontScreenPM.ContentPlayer.Mute();
        }
        else if(_kernStates.Contains(false))
        {
            _controller.HideReadyMessage();
            _readyMessageShown = false;
            if(!_controller.FrontScreenPM.ContentPlayer.isPlaying) _controller.FrontScreenPM.ContentPlayer.UnMute();
        }

        if ((_readyMessageShown && Input.GetKeyDown(KeyCode.Alpha0) && !_isTransitionStarted) ||
            (_readyMessageShown && Input.GetKeyDown(KeyCode.Q) && !_isTransitionStarted))
        {
            _controller.TransitionedNextState();
            _controller.SetNextState();
            _controller.HideReadyMessage();
            _isTransitionStarted = true;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _controller.SetWaitingMode();
            _controller.FrontScreenPM.SetTransparent();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Q))
        {
            AwakeSoundManager.Play("button");
        }
    }
    
    public override void OnExit()
    {
        _controller.FrontScreenPM.Pause();
        _controller.PauseBackgroundSound();
    }
}

public class ContentMode : AbstractSceneState
{
    private bool isCountdownPlayed = false;
    public ContentMode(SceneController controller) : base(controller){}

    public override void OnEnter()
    {
        isCountdownPlayed = false;
        _controller.FrontScreenShowMode.ContentPlayer.Seek(0);
        _controller.DelayAction(_controller.backDelay, () =>
        {
            _controller.BackScreenContent.Open(_controller.VideosFolder, "show_back", false, false);
            _controller.BackScreenContent.Play();
        });
        
        _controller.DelayAction(_controller.frontDelay, () =>
        {
            _controller.FrontScreenPM.SetTransparent();
            _controller.FrontScreenShowMode.SetOpaque();
            _controller.FrontScreenShowMode.Play();
        });
        
        _controller.FrontScreenShowMode.ContentPlayer.onFinished.AddListener(StartTransition);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _controller.FrontScreenShowMode.Pause();
            StartTransition();
        }

        Debug.Log(_controller.FrontScreenShowMode.ContentPlayer.time);
        // Debug.Log(_controller.FrontScreenShowMode.ContentPlayer.duration);
        
        if (_controller.FrontScreenShowMode.ContentPlayer.time >=
            _controller.FrontScreenShowMode.ContentPlayer.duration - 1 && !isCountdownPlayed && _controller.FrontScreenShowMode.ContentPlayer.time < _controller.FrontScreenShowMode.ContentPlayer.duration - 0.1)
        {
            isCountdownPlayed = true;
            AwakeSoundManager.Play("countdown");
        }
    }

    private void StartTransition()
    {
        _controller.TransitionedNextState();
        _controller.SetNextState();
    }

    public override void OnExit()
    {
        _controller.FrontScreenShowMode.ContentPlayer.onFinished.RemoveListener(StartTransition);
    }
    
}