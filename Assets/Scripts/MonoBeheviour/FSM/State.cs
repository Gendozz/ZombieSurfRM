using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine stateMachine;

    protected State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    
    public virtual void Enter()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }
    
    public virtual void Update()
    {
        
    }
    
    public virtual void PhysicUpdate()
    {
        
    }
    
    public virtual void Exit()
    {
        
    }
}
