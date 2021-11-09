// Decompiled with JetBrains decompiler
// Type: Bounty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class Bounty : MonoBehaviour
{
  protected const float UP_POSITION_SHIFT = 0.3f;
  protected const float RAY_DISTANCE = 10f;
  [SerializeField]
  protected Transform model;
  [SerializeField]
  protected ParticleSystem initialEffect;
  [SerializeField]
  protected AudioSource pickUpSound;
  [SerializeField]
  protected ParticleSystem pickUpEffect;
  protected Collider objectCollider;

  protected void Start()
  {
    this.objectCollider = this.GetComponent<Collider>();
    RaycastHit hitInfo;
    if (!Physics.Raycast(new Ray(this.transform.position, Vector3.down), out hitInfo, 10f))
      return;
    Vector3 vector3 = hitInfo.point;
    vector3.y += 0.3f;
    if (hitInfo.collider.tag.Equals(StringConst.Tags.OBSTACLE_COLLIDER))
    {
      Transform parent = hitInfo.transform.parent;
      while ((Object) parent != (Object) null && !parent.tag.Equals(StringConst.Tags.OBSTACLE))
        parent = parent.parent;
      if ((Object) parent == (Object) null)
      {
        Debug.Log((object) string.Format("Obstacle {0} does not has tag {1}", (object) hitInfo.collider.gameObject, (object) StringConst.Tags.OBSTACLE));
        Debug.Break();
        return;
      }
      ObstacleNotMy component = parent.GetComponent<ObstacleNotMy>();
      if ((Object) component == (Object) null)
      {
        Debug.Log((object) string.Format("Obstacle {0} does not has script", (object) parent));
        Debug.Break();
        return;
      }
      if ((Object) component.BountySpawnPosition != (Object) null)
        vector3 = component.BountySpawnPosition.position;
    }
    this.transform.position = vector3;
  }

  protected virtual void OnTriggerEnter(Collider other)
  {
    if (!other.tag.Equals("LiftingObstacleCollider"))
      return;
    this.transform.SetParent(other.transform);
    this.transform.Translate(Vector3.up * 0.3f);
  }

  public void SetRotation(float rotation) => this.model.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
}
