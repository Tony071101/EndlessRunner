using UnityEngine;

public class PlayerCollision : CharacterControllerBase
{

    protected override void Start() {
        base.Start();

        GameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }

    private void ActivatePlayer() {
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Obstacle")) {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("CollectiveRing")) {
            GameManager.Instance.currentRing++;
            Destroy(collision.gameObject);
        }
    }
}
