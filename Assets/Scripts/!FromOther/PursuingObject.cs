// Decompiled with JetBrains decompiler
// Type: PursuingObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PursuingObject : MonoBehaviour
{
  public Transform objToFollow;
  public bool followX = true;
  public bool followY = true;
  public bool followZ = true;
  private Vector3 deltaPos;

  protected virtual void Start() => this.deltaPos = this.transform.position - this.objToFollow.position;

  private void LateUpdate()
  {
    Vector3 position = this.transform.position;
    if (this.followX)
      position.x = this.objToFollow.position.x + this.deltaPos.x;
    if (this.followY)
      position.y = this.objToFollow.position.y + this.deltaPos.y;
    if (this.followZ)
      position.z = this.objToFollow.position.z + this.deltaPos.z;
    this.transform.position = position;
  }

  public void ChangeObject(Transform newObjToFollow)
  {
    this.objToFollow = newObjToFollow;
    this.deltaPos = this.transform.position - newObjToFollow.position;
  }
}
