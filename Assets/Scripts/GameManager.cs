using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float currentScore = 0f;
    public bool isPlaying = false;
    private void Awake() {
        if(Instance == null) Instance = this;  
    }

    private void Update() {
        if(isPlaying) {
            currentScore += Time.deltaTime;
        }
    }

    public void GameOver() {
        currentScore = 0f;
        isPlaying = false;
    }

    public string Score() {
        return Mathf.RoundToInt(currentScore).ToString();
    }
}
