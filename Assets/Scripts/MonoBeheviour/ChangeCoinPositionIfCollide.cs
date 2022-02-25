using UnityEngine;

public class ChangeCoinPositionIfCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if(obstacle != null)
        {
            switch (obstacle.GetType())
            {
                case Obstacle.ObstacleType.LOW:
                    transform.position += Vector3.up;
                    break;
                case Obstacle.ObstacleType.HIGH:
                    break;
                case Obstacle.ObstacleType.HIGH_SOLID:
                    GetComponent<ReplacableObject>().objectIsOutOfSee?.Invoke();
                    gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}
