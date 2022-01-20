using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    protected Player player;

    public PlayerState(Player player, StateMachine stateMachine) : base (stateMachine)
    {
        this.player = player;
    }
}
