using System;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefs;
    [SerializeField] private float obstacleSpeed;
    [SerializeField] [Range(0f, 1f)] private float obstacleSpeedFactor = 0.2f;
    [SerializeField] private float obstacleSpawnTime;
    [SerializeField] [Range(0f, 1f)] private float obstacleSpawnTimeFactor = 0.1f;
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeUntilObstaclesSpawn;
    private float timeAlive;

    private void Start() {
        GameManager.Instance.onPlay.AddListener(ResetFators);
    }

    private void Update() {
        if(GameManager.Instance.isPlaying) {
            timeAlive += Time.deltaTime;
            CalculateFactors();
            SpawnLoop();    
        }
    }

    private void SpawnLoop()
    {
        timeUntilObstaclesSpawn += Time.deltaTime;

        if(timeUntilObstaclesSpawn >= _obstacleSpawnTime) {
            Spawn();
            timeUntilObstaclesSpawn = 0f;
        }
    }

    private void CalculateFactors() {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFators() {
        timeAlive = 1f;    
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefs[UnityEngine.Random.Range(0, obstaclePrefs.Length)];

        GameObject spawnObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstalceRB = spawnObstacle.GetComponent<Rigidbody2D>();
        obstalceRB.linearVelocity = Vector2.left * _obstacleSpeed;
    }
}
