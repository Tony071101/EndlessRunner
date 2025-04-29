using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Data data;
    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public float currentScore = 0f;
    public bool isPlaying = false;
    private void Awake() {
        if(Instance == null) Instance = this;  
    }

    private void Start() {
        string loadedData = SaveSystem.Load("save");
        if(loadedData != null) {
            data = JsonUtility.FromJson<Data>(loadedData);
        } else {
            data = new Data();
        }
    }

    private void Update() {
        if(isPlaying) {
            currentScore += Time.deltaTime;
        }
    }

    public void StartGame() {
        onPlay?.Invoke();
        isPlaying = true;
        currentScore = 0f;
    }

    public void GameOver() {
        if(data.highscore < currentScore) {
            data.highscore = currentScore;

            string saveString = JsonUtility.ToJson(data);
            SaveSystem.Save("save", saveString);
        }
        isPlaying = false;
        onGameOver?.Invoke();
    }

    public string ConvertToIntScore() {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string ConvertToIntHighScore() {
        return Mathf.RoundToInt(data.highscore).ToString();
    }
}
