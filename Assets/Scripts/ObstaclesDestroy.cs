using UnityEngine;

public class ObstaclesDestroy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Obstacle")) {
            Destroy(collision.gameObject);
        }
    }
}
