using System;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefs;
    [SerializeField] private float obstacleSpeed;
    [SerializeField] private float obstacleSpawnTime;
    private float timeUntilObstaclesSpawn;
    private void Update() {
        SpawnLoop();    
    }

    private void SpawnLoop()
    {
        timeUntilObstaclesSpawn += Time.deltaTime;

        if(timeUntilObstaclesSpawn >= obstacleSpawnTime) {
            Spawn();
            timeUntilObstaclesSpawn = 0f;
        }
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefs[UnityEngine.Random.Range(0, obstaclePrefs.Length)];

        GameObject spawnObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstalceRB = spawnObstacle.GetComponent<Rigidbody2D>();
        obstalceRB.linearVelocity = Vector2.left * obstacleSpeed;
    }
}
