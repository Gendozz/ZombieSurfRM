// Decompiled with JetBrains decompiler
// Type: Rotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Rotation : MonoBehaviour
{
  public float speed;
  public Rotation.Axis rotationAxis;

  private void Start()
  {
  }

  private void FixedUpdate()
  {
    Vector3 zero = Vector3.zero;
    switch (this.rotationAxis)
    {
      case Rotation.Axis.X:
        zero.x = 1f;
        break;
      case Rotation.Axis.Y:
        zero.y = 1f;
        break;
      case Rotation.Axis.Z:
        zero.z = 1f;
        break;
    }
    this.transform.Rotate(zero, this.speed);
  }

  public enum Axis
  {
    X,
    Y,
    Z,
  }
}
