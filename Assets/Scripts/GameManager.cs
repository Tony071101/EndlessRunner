using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Data Data;
    public UnityEvent onPlay { get; private set; } = new UnityEvent();
    public UnityEvent onGameOver { get; private set; } = new UnityEvent();
    public float currentScore { get; private set; } = 0f;
    public int currentRing { get; set; } = 0;
    public bool isPlaying { get; private set; } = false;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            Data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
        {
            Data = new Data
            {
                highscore = 0f,
                totalRings = 0,
                characterStates = new List<CharacterSaveData>()
            };
        }
        InitializeCharacterStates(CharacterManager.Instance.GetAllCharacters());
        CharacterManager.Instance.LoadFromGameData(Data);
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        onPlay?.Invoke();
        isPlaying = true;
        currentScore = 0f;
        currentRing = 0;
    }

    public void GameOver()
    {
        bool shouldSave = false;

        if (Data.highscore < currentScore)
        {
            Data.highscore = currentScore;
            shouldSave = true;
        }

        Data.totalRings += currentRing;
        shouldSave = true;

        if (shouldSave)
        {
            string saveString = JsonUtility.ToJson(Data);
            SaveSystem.Save("save", saveString);
        }

        isPlaying = false;
        onGameOver?.Invoke();
    }

    public string ConvertToIntScore()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string ConvertToIntHighScore()
    {
        return Mathf.RoundToInt(Data.highscore).ToString();
    }

    public string GetCurrentRing()
    {
        return currentRing.ToString();
    }

    public string GetTotalRings()
    {
        return Data.totalRings.ToString();
    }

    public void InitializeCharacterStates(List<CharacterInfo> allInfos)
    {
        if (Data.characterStates == null)
        {
            Data.characterStates = new List<CharacterSaveData>();
        }

        foreach (var info in allInfos)
        {
            bool exists = Data.characterStates.Exists(c => c.characterId == info.characterId);
            if (!exists)
            {
                Data.characterStates.Add(new CharacterSaveData
                {
                    characterId = info.characterId,
                    isUnlocked = false,
                    isEquipped = false
                });
            }
        }

        // Đảm bảo ít nhất 1 nhân vật được equipped
        if (!Data.characterStates.Exists(c => c.isEquipped))
        {
            var firstUnlocked = Data.characterStates.Find(c => c.isUnlocked);
            if (firstUnlocked != null)
            {
                firstUnlocked.isEquipped = true;
            }
        }
    }
}
