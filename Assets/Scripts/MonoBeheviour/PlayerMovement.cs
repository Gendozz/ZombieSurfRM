using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Settings
    private float fromLaneToLaneDistance = 3f;

    [SerializeField]
    private float gravity = 20f;

    [SerializeField]
    private float jumpForce = 8f;

    private float changeLaneDuration = 0.5f;

    // Movements

    private float horizontalInput;
    private float verticalInput;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float currentSideDisrection;


    private CharacterController charController;

    private Vector3 currentMovement;



    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if(horizontalInput != 0)
        {
            StartCoroutine(ChangeLane(horizontalInput));
            print($"horizontalInput => {horizontalInput}");
        }
        verticalInput = Input.GetAxisRaw("Vertical");

        //charController.Move(currentMovement);

    }

    IEnumerator ChangeLane(float sideDirection)
    {
        print("ChangeLane started");
        SetNewXPosition(sideDirection);

        float elapsedTime = 0;
        Vector3 currentPosition = transform.position;

        while(elapsedTime < changeLaneDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, endPosition, (elapsedTime / changeLaneDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //transform.position = endPosition;
        yield return null;
    }


    private void SetNewXPosition(float sideDirection)
    {
        startPosition = transform.position;
        endPosition = transform.position + Vector3.right * sideDirection * fromLaneToLaneDistance;
        print($"endPosition => {endPosition}");
    }

}
