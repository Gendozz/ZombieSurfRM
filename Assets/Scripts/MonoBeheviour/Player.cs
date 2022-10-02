using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
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

    private float slideAnimationDuration;

    private float runAnimationDuration;

    private int maxLanesFromCenter = 1;

    private float[] xPositions;

    private readonly Vector3 defaultCharControllerCenter = new Vector3(0f, 1f, 0f);

    private readonly float defaultCharControllerHieght = 2f;
    
    private readonly Vector3 slidingCharControllerCenter = new Vector3(0f, 0.5f, 0f);

    private readonly float slidingCharControllerHeight = 1f;


    // Movements and rotation

    private float horizontalInput;

    private Vector3 startPosition;

    private int currentLaneNumber = 0;

    private Vector3 currentMovement = Vector3.zero;

    private Quaternion defaultRotation;

    [SerializeField]
    private float deflectionAngle;

    private int previousSideDirection = 0;

    // Animation

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform playerModelTransform;

    // Ragdoll

    private Collider[] ragdollColliders;

    private Rigidbody[] ragdollRigidbodies;

    private CharacterJoint[] characterJoints;

    [SerializeField]
    private float deathThrowBackForce = 15f;

    // Other

    private Coroutine changeLaneRoutine = null;

    private Coroutine jumpRoutine = null;

    private Coroutine slideRoutine = null;

    private Coroutine setCapsuleToDefaultRoutine = null;
    
    private CharacterController charController;

    private bool isAlive = true;

    // Audioclips
    [SerializeField]
    private AudioClip slidingAudio;


    private void Start()
    {
        charController = GetComponent<CharacterController>();

        defaultRotation = playerModelTransform.rotation;

        xPositions = new float[] { -laneToLaneDistance, 0, laneToLaneDistance };

        RuntimeAnimatorController RTAController = animator.runtimeAnimatorController;

        for (int clipNumber = 0; clipNumber < RTAController.animationClips.Length; clipNumber++)
        {
            string animationName = RTAController.animationClips[clipNumber].name;

            if (animationName.Equals("MoveRight"))
            {
                changeLaneDuration = RTAController.animationClips[clipNumber].length;
            }
            if (animationName.Equals("Slide"))
            {
                slideAnimationDuration = RTAController.animationClips[clipNumber].length;
            }
        }


        // Ragdoll

        CashRagdollComponents();
        ChangeRagdollActivenessTo(false);
    }

    private void Update()
    {        
        if (!isAlive)
        {
            return;
        }
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
                previousSideDirection = (int)horizontalInput;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            if (charController.isGrounded)
            {
                jumpRoutine = StartCoroutine(Jump());
            }
            return;
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
            setCapsuleToDefaultRoutine = StartCoroutine(SetCapsuleColliderToDefaults(0));
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

    private IEnumerator Slide()
    {
        if(jumpRoutine != null)
        {
            StopCoroutine(jumpRoutine);
        }

        if(setCapsuleToDefaultRoutine != null)
        {
            StopCoroutine(setCapsuleToDefaultRoutine);
        }

        currentGravity = defaultGravity;

        charController.height = slidingCharControllerHeight;
        charController.center = slidingCharControllerCenter;

        animator.SetTrigger(Constants.AnimationParameters.SLIDE_TRIG);

        setCapsuleToDefaultRoutine = StartCoroutine(SetCapsuleColliderToDefaults(slideAnimationDuration));

        SoundManager.Instance.Play(slidingAudio);

        yield return null;
    }

    public void KillPlayer()
    {
        StopAllCoroutines();
        isAlive = false;
        //animator.SetTrigger(Constants.AnimationParameters.DEATH_TRIG);

        // Ragdoll
        charController.enabled = false;
        animator.enabled = false;
        ChangeRagdollActivenessTo(true);
    }

    public bool AreYouAlive()
    {
        return isAlive;
    }

    private IEnumerator SetCapsuleColliderToDefaults(float setDelay)
    {
        yield return new WaitForSeconds(setDelay);

        charController.height = defaultCharControllerHieght;
        charController.center = defaultCharControllerCenter;
    }

    public void HandleSideHit()
    {
        animator.SetBool(Constants.AnimationParameters.SIDEHIT_TRIG, true);
        StopCoroutine(changeLaneRoutine);
        changeLaneRoutine = StartCoroutine(ChangeLane(previousSideDirection * -1));
    }

    #region Ragdoll

    private void CashRagdollComponents()
    {
        ragdollColliders = playerModelTransform.gameObject.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = playerModelTransform.gameObject.GetComponentsInChildren<Rigidbody>();
        characterJoints = playerModelTransform.gameObject.GetComponentsInChildren<CharacterJoint>();
    }

    private void ChangeRagdollActivenessTo(bool isActive)
    {
        foreach (CharacterJoint characterJoint in characterJoints)
        {
            characterJoint.enableProjection = isActive;
        }

        foreach (Collider collider in ragdollColliders)
        {
            if (!collider.tag.Equals("Footstep"))
            {
                collider.enabled = isActive; 
            }
        }

        foreach (Rigidbody rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = !isActive;
            rigidbody.useGravity = isActive;
            rigidbody.AddForce(-Vector3.forward * deathThrowBackForce, ForceMode.Impulse);
        }
    }
    #endregion
}
