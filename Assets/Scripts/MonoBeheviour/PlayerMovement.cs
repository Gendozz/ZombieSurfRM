using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Settings
    private float laneToLaneDistance = 3f;

    [SerializeField]
    private float currentGravity = 60f;

    [SerializeField]
    private float defaultGravity = 60f;

    [SerializeField]
    private float jumpGravity = 20f;

    [SerializeField]
    private float jumpForce = 8f;

    [SerializeField]
    private float changeLaneDuration = 0.2f;

    private int maxLanesFromCenter = 1;

    private float[] xPositions;

    // Movements

    private float horizontalInput;

    private Vector3 startPosition;

    private int currentLaneNumber = 0;

    private Vector3 currentMovement = Vector3.zero;

    private Quaternion defaultRotation;

    [SerializeField]
    private float deflectionAngle;

    // Flags

    private bool isChangingLane = false;

    private CharacterController charController;

    // Animation

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform playerModelTransform;

    // Other

    Coroutine changeLaneRoutine = null;

    Coroutine jumpRoutine = null;

    Coroutine slideRoutine = null;


    private void Start()
    {
        charController = GetComponent<CharacterController>();

        defaultRotation = playerModelTransform.rotation;

        xPositions = new float[] { -laneToLaneDistance, 0, laneToLaneDistance };

        RuntimeAnimatorController RTAController = animator.runtimeAnimatorController;

        for (int clipNumber = 0; clipNumber < RTAController.animationClips.Length; clipNumber++)
        {
            if (RTAController.animationClips[clipNumber].name.Equals("MoveRight"))
            {
                changeLaneDuration = RTAController.animationClips[clipNumber].length;
            }
        }
    }

    private void Update()
    {
        HandleInput();
        Move();
    }

    private void HandleInput()
    {
        horizontalInput = 0f;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        if (horizontalInput != 0)
        {
            bool isLaneInDirection = Mathf.Abs(currentLaneNumber + horizontalInput) <= maxLanesFromCenter;

            if (isLaneInDirection)
            {
                if (changeLaneRoutine != null)
                {
                    StopCoroutine(changeLaneRoutine);
                }
                changeLaneRoutine = StartCoroutine(ChangeLane((int)horizontalInput));
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            if (charController.isGrounded)
            {
                jumpRoutine = StartCoroutine(Jump());
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            slideRoutine = StartCoroutine(Slide());
        }
    }

    private void Move()
    {
        currentMovement.y -= (currentGravity * Time.deltaTime);
        charController.Move(currentMovement * Time.deltaTime);
    }

    private IEnumerator ChangeLane(int sideDirection)
    {
        // Position
        startPosition = transform.position;
        currentLaneNumber += sideDirection;
        float newXPosition = xPositions[currentLaneNumber + 1];
        float currentXPosition;

        // Rotation
        Quaternion newRotation = Quaternion.Euler(
            playerModelTransform.rotation.x,
            playerModelTransform.rotation.y + (deflectionAngle * sideDirection),
            playerModelTransform.rotation.z);

        // Common
        float elapsedTime = 0f;
        float fraction = 0f;
        animator.SetInteger(Constants.AnimationParameters.SIDEMOVE_INT, sideDirection);

        while (elapsedTime < changeLaneDuration)
        {
            //Positon
            currentXPosition = Mathf.Lerp(startPosition.x, newXPosition, fraction);
            transform.position = new Vector3(currentXPosition, transform.position.y, transform.position.z);
            elapsedTime += Time.deltaTime;

            // Rotation
            if (fraction <= 0.5f)
            {
                playerModelTransform.rotation = Quaternion.Slerp(defaultRotation, newRotation, fraction);
            }
            else
            {
                playerModelTransform.rotation = Quaternion.Slerp(newRotation, defaultRotation, fraction);
            }

            // Common
            fraction = elapsedTime / changeLaneDuration;


            yield return null;
        }
        animator.SetInteger(Constants.AnimationParameters.SIDEMOVE_INT, 0);
        animator.SetTrigger(Constants.AnimationParameters.LANECHANGED_TRIG);

        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        playerModelTransform.rotation = defaultRotation;
    }


    private IEnumerator Jump()
    {
        if(slideRoutine != null)
        {
            StopCoroutine(slideRoutine);
        }
        currentGravity = jumpGravity;
        currentMovement.y = jumpForce;
        animator.SetTrigger(Constants.AnimationParameters.JUMP_TRIG);

        yield return new WaitForSeconds(0.1f);

        float previousY = 0;
        float currentY = 0;
        while (!charController.isGrounded)
        {
            previousY = currentY;
            currentY = transform.position.y;
            if (currentY < previousY)
            {
                currentGravity = defaultGravity;
            }

            yield return null;
        }
        animator.SetTrigger(Constants.AnimationParameters.LANDED_TRIG);
    }

    IEnumerator Slide()
    {
        if(jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine);
        }
        currentGravity = defaultGravity;

        animator.SetTrigger(Constants.AnimationParameters.SLIDE_TRIG);

        

        yield return null;
    }
}
