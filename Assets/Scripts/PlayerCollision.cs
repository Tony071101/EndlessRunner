using UnityEngine;

public class PlayerCollision : CharacterControllerBase
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Obstacle")) {
            Destroy(gameObject);
        }
    }
}
