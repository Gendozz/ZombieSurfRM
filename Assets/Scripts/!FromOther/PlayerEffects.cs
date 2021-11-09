// Decompiled with JetBrains decompiler
// Type: PlayerEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
  [SerializeField]
  private ParticleSystem hitEffect;
  [SerializeField]
  private ParticleSystem[] jumpBonusEffects;

  public void PlayHitEffect(Vector3 position)
  {
    this.hitEffect.transform.position = position;
    this.hitEffect.Play();
  }

  public void StartBonusEffect()
  {
    for (int index = 0; index < this.jumpBonusEffects.Length; ++index)
      this.jumpBonusEffects[index].Play();
  }

  public void StopBonusEffect()
  {
    for (int index = 0; index < this.jumpBonusEffects.Length; ++index)
      this.jumpBonusEffects[index].Stop();
  }
}
