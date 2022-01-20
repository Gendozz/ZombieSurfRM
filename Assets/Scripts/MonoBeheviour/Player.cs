using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public class CurrentProps
    {
        [SerializeField]
        private float gravity;

        [SerializeField]
        private float jumpForce;

        public bool canJump;
    }
    
    private CharacterController charController;

    public StateMachine movementStateMachine;
    public GroundedState groundedState;
    public JumpingState jumpingState;
    public ChangingLaneState changingLaneState;
    public SlidingState slidingState;

    public void Move(float direction)
    {
        Vector3 movement = new Vector3(direction, 0, 0);
        charController.Move(movement);
    }


    private void Start()
    {
        charController = GetComponent<CharacterController>();

        movementStateMachine = new StateMachine();

        groundedState = new GroundedState(this, movementStateMachine);
        jumpingState = new JumpingState(this, movementStateMachine);
        changingLaneState = new ChangingLaneState(this, movementStateMachine);
        slidingState = new SlidingState(this, movementStateMachine);

        movementStateMachine.Init(groundedState);

    }

    private void Update()
    {
        movementStateMachine.CurrentState.HandleInput();
        movementStateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.CurrentState.PhysicUpdate();
    }

}
