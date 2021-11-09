// Decompiled with JetBrains decompiler
// Type: MobileCraneTrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobileCraneTrap : Trap
{
  [SerializeField]
  private Animator[] craneAnimators;
  [SerializeField]
  private GameObject wall;
  [SerializeField]
  private float wallChance = 0.5f;
  private System.Random random;
  private Animator[] activeAnimators;

  protected override void Start()
  {
    base.Start();
    foreach (Component craneAnimator in this.craneAnimators)
      craneAnimator.gameObject.SetActive(false);
    this.wall.SetActive(false);
    this.random = RandomProvider.GetThreadRandom();
    if (this.random.NextDouble() > 0.5)
    {
      this.activeAnimators = new Animator[2];
      int index1;
      int index2;
      if (this.random.NextDouble() > 0.5)
      {
        index1 = 0;
        index2 = 1;
      }
      else
      {
        index1 = 1;
        index2 = 0;
      }
      this.activeAnimators[0] = this.craneAnimators[index1];
      this.activeAnimators[1] = this.craneAnimators[index2];
    }
    else
    {
      int index = this.random.NextDouble() > 0.5 ? 0 : 1;
      this.activeAnimators = new Animator[1];
      this.activeAnimators[0] = this.craneAnimators[index];
    }
    if (this.random.NextDouble() > 0.5)
      this.activeAnimators[0].transform.position = this.activeAnimators[0].transform.position - 2f * this.activeAnimators[0].transform.right;
    foreach (Component activeAnimator in this.activeAnimators)
      activeAnimator.gameObject.SetActive(true);
    if (this.random.NextDouble() <= (double) this.wallChance)
      return;
    this.wall.SetActive(true);
  }

  protected override void HandleActivateTrap()
  {
    foreach (Animator activeAnimator in this.activeAnimators)
    {
      if (activeAnimator.gameObject.activeSelf)
        activeAnimator.SetTrigger("Turning");
    }
  }
}
