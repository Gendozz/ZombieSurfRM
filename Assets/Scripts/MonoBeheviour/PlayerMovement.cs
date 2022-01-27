using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Settings
    private float laneToLaneDistance = 3f;

    [SerializeField]
    private float currentGravity = 20f;

    [SerializeField]
    private readonly float normalGravity = 20f;
    
    [SerializeField]
    private readonly float fallingGravity = 60f;

    [SerializeField]
    private float jumpForce = 8f;

    [SerializeField]
    private float changeLaneDuration = 0.2f;

    private int maxLanesFromCenter = 1;

    // Movements

    private float horizontalInput;
    private float verticalInput;

    private Vector3 startPosition;
    private Vector3 endPosition;

    [SerializeField]
    private int currentLaneNumber = 0;

    private float currentSideDirection;

    private Vector3 currentMovement = Vector3.zero;

    // Flags

    private bool isChangingLane = false;


    private CharacterController charController;




    private void Start()
    {
        charController = GetComponent<CharacterController>();
        
        // Push character controller to floor
        //currentMovement.y = -1f;
    }

    private void Update()
    {
        HandleInput();
        Move();

    }

    private void Move()
    {
        currentMovement.y -= (currentGravity * Time.deltaTime);
        charController.Move(currentMovement * Time.deltaTime);
    }

    private void HandleInput()
    {
        horizontalInput = 0f;
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        if (horizontalInput != 0)
        {
            bool isLaneInDirection = Mathf.Abs(currentLaneNumber + horizontalInput) <= maxLanesFromCenter;

            if (isLaneInDirection)
            {
                StartCoroutine(ChangeLane(horizontalInput));                
            }
        }

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.Space))
        {
            if (charController.isGrounded)
            {
                StartCoroutine(Jump());   
            }           
        }
        
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Slide());
        }
    }


    private IEnumerator ChangeLane(float sideDirection)
    {
        //for (float waitTime = 0.2f; waitTime > 0.0 && isChangingLane; waitTime -= Time.fixedDeltaTime)
        //    yield return null;



        if (!isChangingLane)
        {
            isChangingLane = true;
            currentLaneNumber += (int)sideDirection;

            startPosition = transform.position;

            float newXPosition = transform.position.x + (sideDirection * laneToLaneDistance);
            float elapsedTime = 0;
            float currentXPosition;

            while (elapsedTime < changeLaneDuration)
            {
                currentXPosition = Mathf.Lerp(startPosition.x, newXPosition, (elapsedTime / changeLaneDuration));
                transform.position = new Vector3(currentXPosition, transform.position.y, transform.position.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

            isChangingLane = false;
        }
    }



    IEnumerator Jump()
    {            
        currentMovement.y = jumpForce;
        //print($"Jump coroutine started. curentMovement => {currentMovement} ");

        do
        {
            //if(gameObj)

            yield return new WaitForFixedUpdate();
        } while (!charController.isGrounded);
        //print($"Jump coroutine ended. curentMovement => {currentMovement} ");
        yield return null;
    }

    IEnumerator Slide()
    {
        yield return null;
    }
}
