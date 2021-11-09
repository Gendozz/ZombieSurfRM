// Decompiled with JetBrains decompiler
// Type: Billboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Billboard : ObstacleNotMy
{
  [SerializeField]
  private SkinnedMeshRenderer skinnedMeshRenderer;

  public void ChangeTexture(Texture texture)
  {
    this.skinnedMeshRenderer.gameObject.SetActive(false);
    this.skinnedMeshRenderer.material.mainTexture = texture;
    float num = (float) texture.width / (float) texture.height;
    Vector3 localScale = this.skinnedMeshRenderer.transform.localScale;
    if ((double) num > 1.0)
      localScale.z = 1f / num;
    else
      localScale.x = num;
    this.skinnedMeshRenderer.transform.localScale = localScale;
    this.skinnedMeshRenderer.gameObject.SetActive(true);
    this.skinnedMeshRenderer.localBounds = this.skinnedMeshRenderer.sharedMesh.bounds;
  }
}
