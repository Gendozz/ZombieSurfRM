using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    private float horizontalInput;
    private float verticalInput;

    public GroundedState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public override void Update()
    {
        base.Update();
        player.Move(horizontalInput);
    }
}
