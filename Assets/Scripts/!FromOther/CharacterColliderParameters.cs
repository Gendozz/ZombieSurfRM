// Decompiled with JetBrains decompiler
// Type: CharacterColliderParameters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct CharacterColliderParameters
{
  public Vector3 center;
  public float radius;
  public float height;

  public void SetByCharacterController(CharacterController cc)
  {
    this.center = cc.center;
    this.radius = cc.radius;
    this.height = cc.height;
  }

  public void SetByCapsuleCollider(CapsuleCollider collider)
  {
    this.center = collider.center;
    this.radius = collider.radius;
    this.height = collider.height;
  }

  public void AssignToCharacterController(CharacterController cc)
  {
    cc.center = this.center;
    cc.radius = this.radius;
    cc.height = this.height;
  }
}
