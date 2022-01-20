// Decompiled with JetBrains decompiler
// Type: Animations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class Animations
{
  public static float GetAnimationClipLength(Animator ac, string animationClipName)
  {
    AnimationClip[] animationClips = ac.runtimeAnimatorController.animationClips;
    for (int index = 0; index < animationClips.Length; ++index)
    {
      if (animationClips[index].name.Equals(animationClipName))
        return animationClips[index].length;
    }
    return 0.0f;
  }

  public static class Parameters
  {
    public static string IS_ALIVE = "IsAlive";
    public static string IS_GROUNDED = "IsGrounded";
    public static string STRAFE_RIGHT = "StrafeRight";
    public static string STRAFE_LEFT = "StrafeLeft";
    public static string INTERRUPT_STRAFE = "InterruptStrafe";
    public static string JUMP_TRIGGER = "JumpTrigger";
    public static string JUMPING = "Jumping";
    public static string ROLL_TRIGGER = "RollTrigger";
    public static string ROLLING = "Rolling";
  }

  public static class Names
  {
    public static string DEATH_STANDING = "StandingDeath";
    public static string DEATH_ROLLING = "SlidingDeath";
    public static string STRAFE_RIGHT = "RightFootStep";
    public static string STRAFE_LEFT = "LeftFootStep";
    public static string INTERRUPTED_STRAFE_LEFT = "Hit On Side Of Body Left";
    public static string INTERRUPTED_STRAFE_RIGHT = "Hit On Side Of Body Right";
  }
}
