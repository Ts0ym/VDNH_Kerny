using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSceneState
{
    protected AbstractSceneState(SceneController controller)
    {
        _controller = controller;
    }
    
    protected SceneController _controller;
    
    public virtual void OnEnter(){}
    public virtual void OnUpdate(){}
    public virtual void OnExit(){}
    
}
