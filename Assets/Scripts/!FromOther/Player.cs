// Decompiled with JetBrains decompiler
// Type: Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  private const float GROUND_CHECK_RAY_LENGTH = 0.1f;
  private const float JUMP_CHECK_RAY_LENGTH = 0.2f;
  private const float STOP_TIME_BEFORE_DEATH = 0.1f;
  [SerializeField]
  private int playerInitialLaneIndex = 1;
  [SerializeField]
  private float playerLiftTime = 0.2f;
  [SerializeField]
  private float playerJumpHeight = 1.5f;
  [SerializeField]
  private float playerSlideTime = 3f;
  [Tooltip("Скорости персонажа в порядке достижения (м/с)")]
  [SerializeField]
  private List<float> speedList;
  [Tooltip("Время перехода от предыдущей скорости к следующей (сек)")]
  [SerializeField]
  private List<float> speedTransitionDurationList;
  [SerializeField]
  private GameObject playerModel;
  [SerializeField]
  private float maxRotationAngle = 70f;
  [SerializeField]
  private float waitForActionTime = 0.5f;
  [SerializeField]
  private float jumpBonusMultiplier = 1.5f;
  [SerializeField]
  private float jumpBonusTime = 10f;
  [SerializeField]
  private Cloth cloth;
  private GameArea ga;
  private CharacterController cc;
  private Animator ac;
  private PlayerEffects pe;
  private bool isGrounded;
  private bool isGroundedForJump;
  private bool isStrafing;
  private bool isStrafeInterrupted;
  private bool isAlive;
  private float currentStopTime;
  private int currentLaneIndex = 1;
  private Vector3 currentLanePosition = Vector3.zero;
  private Vector3 currentMovingVector = Vector3.zero;
  private float previousZCoord;
  private float jumpGravity;
  private float fallGravity;
  private float playerJumpSpeed;
  private bool jumpBonusAdded;
  private float currentBonusTime;
  private CharacterColliderParameters stayingColliderParameters;
  private CharacterColliderParameters slidingColliderParameters;
  private float strafeClipLength;
  private float interruptedStrafeClipLength;
  private float deathClipLength;
  private float playerSpeed;
  private Collider[] ragdollColliders;
  private Rigidbody[] ragdollRb;

  public static Player Instance { get; private set; }

  private void Awake()
  {
    if ((Object) Player.Instance == (Object) null)
      Player.Instance = this;
    else
      Object.Destroy((Object) this.gameObject);
  }

  private void OnDestroy()
  {
    if (!((Object) Player.Instance == (Object) this))
      return;
    Player.Instance = (Player) null;
    Events.DeleteOnCollectJumpBonusListener(new Events.CollectJumpBonus(this.HandleAddJumpBonus));
  }

  private void Start()
  {
    this.cc = this.GetComponent<CharacterController>();
    this.ac = this.GetComponent<Animator>();
    this.pe = this.GetComponent<PlayerEffects>();
    this.ga = GameArea.Instance;
    this.FillRagdollObjects();
    this.SetEnabledRagdollObjects(false);
    this.currentLaneIndex = this.playerInitialLaneIndex;
    this.currentLanePosition = this.ga.GetLanePosition(this.currentLaneIndex);
    this.previousZCoord = this.transform.position.z - 1f;
    this.playerJumpSpeed = this.playerJumpHeight / this.playerLiftTime;
    this.jumpGravity = (float) (-(double) this.playerJumpSpeed * (double) this.playerJumpSpeed / (2.0 * (double) this.playerJumpHeight));
    this.fallGravity = this.jumpGravity / 2f;
    this.stayingColliderParameters.SetByCharacterController(this.cc);
    this.slidingColliderParameters.SetByCapsuleCollider(this.GetComponent<CapsuleCollider>());
    this.isGrounded = this.IsGrounded(0.1f);
    this.isGroundedForJump = this.IsGrounded(0.2f);
    this.isAlive = true;
    this.ac.SetBool(Animations.Parameters.IS_ALIVE, true);
    this.strafeClipLength = Animations.GetAnimationClipLength(this.ac, Animations.Names.STRAFE_LEFT);
    this.interruptedStrafeClipLength = Animations.GetAnimationClipLength(this.ac, Animations.Names.INTERRUPTED_STRAFE_LEFT);
    this.deathClipLength = Animations.GetAnimationClipLength(this.ac, Animations.Names.DEATH_STANDING);
    Events.AddOnCollectJumpBonusListener(new Events.CollectJumpBonus(this.HandleAddJumpBonus));
    this.StartCoroutine(this.RunSpeedChangingCycle());
  }

  private void Update()
  {
    if (!this.isAlive)
      return;
    float y1 = this.playerModel.transform.rotation.eulerAngles.y;
    float y2;
    if ((double) this.currentMovingVector.x != 0.0)
    {
      float num = 1.5f * this.ga.LaneWidth;
      y2 = (float) (0.5 * (double) this.currentMovingVector.x / (double) (num / this.strafeClipLength) + 0.5 * ((double) this.currentLanePosition.x - (double) this.transform.position.x) / (double) num) * this.maxRotationAngle;
    }
    else
      y2 = 0.0f;
    if ((double) y1 != (double) y2)
      this.playerModel.transform.rotation = Quaternion.Slerp(Quaternion.Euler(0.0f, y1, 0.0f), Quaternion.Euler(0.0f, y2, 0.0f), Time.deltaTime * 10f);
    float direction = 0.0f;
    float num1 = 0.0f;
    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
      direction = -1f;
    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
      direction = 1f;
    else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
      num1 = 1f;
    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.C) || (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftControl)) || Input.GetKeyDown(KeyCode.RightControl))
      num1 = -1f;
    if ((double) direction != 0.0)
      this.StartCoroutine(this.Strafe(direction));
    if ((double) num1 > 0.0)
    {
      this.StartCoroutine(this.Jump());
    }
    else
    {
      if ((double) num1 >= 0.0)
        return;
      this.StartCoroutine(this.Roll());
    }
  }

  private void FixedUpdate()
  {
    if ((double) this.transform.position.y < 0.0199999995529652)
    {
      Debug.Log((object) string.Format("Underground. Y = {0}", (object) this.transform.position.y));
      Vector3 position = this.transform.position;
      position.y = 0.0f;
      this.transform.position = position;
    }
    if (!this.isAlive)
      return;
    this.currentMovingVector.z = this.playerSpeed;
    if (this.isGrounded)
    {
      if ((double) this.currentMovingVector.y < 0.0)
        this.currentMovingVector.y = 0.0f;
    }
    else if ((double) this.currentMovingVector.y > 0.0)
      this.currentMovingVector.y += this.jumpGravity * Time.fixedDeltaTime;
    else
      this.currentMovingVector.y += this.fallGravity * Time.fixedDeltaTime;
    if (this.isAlive && !this.isStrafing)
    {
      float f = this.currentLanePosition.x - this.transform.position.x;
      if ((double) Mathf.Abs(f) > 1.0 / 1000.0)
      {
        if ((double) this.currentMovingVector.x == 0.0 || (double) f * (double) this.currentMovingVector.x < 0.0)
          this.currentMovingVector.x = f / this.interruptedStrafeClipLength;
      }
      else
        this.currentMovingVector.x = 0.0f;
    }
    int num = (int) this.cc.Move(this.currentMovingVector * Time.fixedDeltaTime);
    this.isGrounded = this.IsGrounded(0.1f);
    this.isGroundedForJump = this.IsGrounded(0.2f);
    this.ac.SetBool(Animations.Parameters.IS_GROUNDED, this.isGrounded);
    Events.CallOnPlayerMove(this.transform.position);
  }

  public void Kill()
  {
    this.pe.PlayHitEffect(this.transform.position + this.cc.center);
    this.cc.enabled = false;
    this.ac.enabled = false;
    this.SetEnabledRagdollObjects(true);
    this.isAlive = false;
    this.currentMovingVector = Vector3.zero;
    this.DoGameOverActions();
  }

  private void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if (!this.isAlive || hit.collider.tag.Equals(StringConst.Tags.ROAD))
      return;
    if ((double) Mathf.Abs(this.transform.position.z - this.previousZCoord) < 0.00999999977648258)
    {
      if (this.jumpBonusAdded)
      {
        this.currentMovingVector.y = this.playerJumpSpeed;
        this.currentBonusTime = 0.0f;
        this.currentStopTime = 0.0f;
        this.previousZCoord = this.transform.position.z - 1f;
        return;
      }
      if ((double) this.currentStopTime >= 0.100000001490116)
      {
        this.Kill();
        return;
      }
      this.currentStopTime += Time.fixedDeltaTime;
    }
    else
      this.currentStopTime = 0.0f;
    this.previousZCoord = this.transform.position.z;
    if (!this.isStrafing || this.isStrafeInterrupted || (hit.collider.tag.Equals(StringConst.Tags.ROAD) || (double) hit.moveDirection.x * (double) hit.normal.x >= 0.0))
      return;
    double playerShiftAfterTime = (double) this.GetPlayerShiftAfterTime(this.strafeClipLength);
    Vector3 origin = this.transform.position + this.cc.center + new Vector3(0.0f, (float) (-(double) this.cc.height / 2.0) + this.cc.stepOffset, this.cc.radius);
    Vector3 vector3 = new Vector3(Mathf.Sign(hit.moveDirection.x) * this.ga.LaneWidth, 0.0f, 0.0f);
    Vector3 direction = vector3;
    double magnitude = (double) vector3.magnitude;
    if (!Physics.Raycast(origin, direction, (float) magnitude))
      return;
    this.isStrafeInterrupted = true;
  }

  private void HandleAddJumpBonus() => this.StartCoroutine(this.AddJumpBonus());

  private IEnumerator Strafe(float direction)
  {
    Player player = this;
    for (float waitTime = player.waitForActionTime; (double) waitTime > 0.0 && player.isStrafing; waitTime -= Time.fixedDeltaTime)
      yield return (object) new WaitForFixedUpdate();
    if (!player.isStrafing)
    {
      int newLaneIndex = Mathf.Clamp(player.currentLaneIndex + (int) direction, 0, player.ga.LaneCount - 1);
      if (newLaneIndex != player.currentLaneIndex)
      {
        player.isStrafing = true;
        player.isStrafeInterrupted = false;
        player.ac.SetBool(Animations.Parameters.INTERRUPT_STRAFE, false);
        if ((double) direction > 0.0)
        {
          player.ac.SetBool(Animations.Parameters.STRAFE_RIGHT, true);
          player.ac.SetBool(Animations.Parameters.STRAFE_LEFT, false);
        }
        else if ((double) direction < 0.0)
        {
          player.ac.SetBool(Animations.Parameters.STRAFE_RIGHT, false);
          player.ac.SetBool(Animations.Parameters.STRAFE_LEFT, true);
        }
        int previousLaneIndex = player.currentLaneIndex;
        Vector3 previousLanePosition = player.currentLanePosition;
        player.currentLaneIndex = newLaneIndex;
        player.currentLanePosition = player.ga.GetLanePosition(newLaneIndex);
        float currentStrafeDistance = Mathf.Abs(player.transform.position.x - player.currentLanePosition.x);
        float num = currentStrafeDistance / player.strafeClipLength;
        float distPerFrame = Time.fixedDeltaTime * num;
        player.currentMovingVector.x = direction * num;
        while (!player.isStrafeInterrupted && (double) currentStrafeDistance > 0.0)
        {
          currentStrafeDistance = Mathf.Abs(player.transform.position.x - player.currentLanePosition.x);
          currentStrafeDistance -= distPerFrame;
          if ((double) currentStrafeDistance < 0.0)
            player.currentMovingVector.x = (player.currentLanePosition.x - player.transform.position.x) / Time.fixedDeltaTime;
          yield return (object) new WaitForFixedUpdate();
        }
        player.isStrafing = false;
        player.ac.SetBool(Animations.Parameters.STRAFE_RIGHT, false);
        player.ac.SetBool(Animations.Parameters.STRAFE_LEFT, false);
        if (player.isAlive)
        {
          player.currentMovingVector.x = 0.0f;
          if (player.isStrafeInterrupted)
          {
            player.currentLaneIndex = previousLaneIndex;
            player.currentLanePosition = previousLanePosition;
            player.ac.SetBool(Animations.Parameters.INTERRUPT_STRAFE, true);
            yield return (object) new WaitForSeconds(player.interruptedStrafeClipLength);
            player.isStrafeInterrupted = false;
            player.ac.SetBool(Animations.Parameters.INTERRUPT_STRAFE, false);
          }
          else
          {
            player.currentLaneIndex = newLaneIndex;
            player.currentLanePosition = player.ga.GetLanePosition(player.currentLaneIndex);
          }
        }
      }
    }
  }

  private IEnumerator Jump()
  {
    for (float waitTime = this.waitForActionTime; (double) waitTime > 0.0 && !this.isGroundedForJump; waitTime -= Time.fixedDeltaTime)
      yield return (object) new WaitForFixedUpdate();
    if (this.isGroundedForJump)
    {
      this.ac.SetTrigger(Animations.Parameters.JUMP_TRIGGER);
      this.ac.SetBool(Animations.Parameters.JUMPING, true);
      this.currentMovingVector.y = this.playerJumpSpeed;
      do
      {
        yield return (object) new WaitForFixedUpdate();
      }
      while (!this.isGrounded && this.isAlive);
      this.ac.SetBool(Animations.Parameters.JUMPING, false);
    }
  }

  private IEnumerator Roll()
  {
    this.ac.SetBool(Animations.Parameters.ROLLING, true);
    this.ac.SetTrigger(Animations.Parameters.ROLL_TRIGGER);
    for (float waitTime = this.waitForActionTime; (double) waitTime > 0.0 && !this.isGrounded; waitTime -= Time.fixedDeltaTime)
    {
      this.currentMovingVector.y += 2f * this.fallGravity * Time.fixedDeltaTime;
      yield return (object) new WaitForFixedUpdate();
    }
    this.slidingColliderParameters.AssignToCharacterController(this.cc);
    float slideTimeRemaining = this.playerSlideTime;
    do
    {
      yield return (object) new WaitForFixedUpdate();
      slideTimeRemaining -= Time.deltaTime;
    }
    while (this.isGrounded && this.isAlive && (double) slideTimeRemaining > 0.0);
    if (this.isAlive)
      this.ac.SetBool(Animations.Parameters.ROLLING, false);
    this.stayingColliderParameters.AssignToCharacterController(this.cc);
  }

  private IEnumerator RunSpeedChangingCycle()
  {
    if (this.speedList.Count < 2)
    {
      this.playerSpeed = this.speedList[0];
    }
    else
    {
      for (int currentSpeedIndex = 0; this.isAlive && currentSpeedIndex < this.speedList.Count - 1; ++currentSpeedIndex)
      {
        float accelerationPerFrame = (this.speedList[currentSpeedIndex + 1] - this.speedList[currentSpeedIndex]) / this.speedTransitionDurationList[currentSpeedIndex] * Time.fixedDeltaTime;
        this.playerSpeed = this.speedList[currentSpeedIndex];
        for (float nextSpeed = this.speedList[currentSpeedIndex + 1]; this.isAlive && (double) this.playerSpeed < (double) nextSpeed; this.playerSpeed += accelerationPerFrame)
          yield return (object) new WaitForFixedUpdate();
      }
    }
  }

  private IEnumerator AddJumpBonus()
  {
    this.currentBonusTime = this.jumpBonusTime;
    if (!this.jumpBonusAdded)
    {
      this.jumpBonusAdded = true;
      this.pe.StartBonusEffect();
      float previousJumpSpeed = this.playerJumpSpeed;
      this.playerJumpSpeed *= this.jumpBonusMultiplier;
      for (; (double) this.currentBonusTime > 0.0; this.currentBonusTime -= Time.deltaTime)
        yield return (object) null;
      this.playerJumpSpeed = previousJumpSpeed;
      this.pe.StopBonusEffect();
      this.jumpBonusAdded = false;
    }
  }

  private bool IsGrounded(float rayLength) => Physics.Raycast(this.transform.position + this.cc.center + new Vector3(0.0f, (float) (-(double) this.cc.height / 2.0 + (double) this.cc.stepOffset + 0.00999999977648258)), Vector3.down, (float) ((double) this.cc.stepOffset + (double) rayLength + 0.00999999977648258));

  private void DoGameOverActions() => Events.CallOnGameOver();

  public float GetPlayerShiftAfterTime(float time) => this.playerSpeed * time;

  public void ChangePlayerPosition(Vector3 shiftVector)
  {
    this.cc.enabled = false;
    this.cloth.enabled = false;
    this.transform.position += shiftVector;
    this.cloth.enabled = true;
    this.cc.enabled = true;
  }

  public void FillRagdollObjects()
  {
    this.ragdollColliders = this.playerModel.GetComponentsInChildren<Collider>();
    this.ragdollRb = this.playerModel.GetComponentsInChildren<Rigidbody>();
  }

  public void SetEnabledRagdollObjects(bool enabled)
  {
    foreach (Collider ragdollCollider in this.ragdollColliders)
      ragdollCollider.enabled = enabled;
    foreach (Rigidbody rigidbody in this.ragdollRb)
      rigidbody.isKinematic = !enabled;
  }
}
