using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI ringUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;
    [SerializeField] private TextMeshProUGUI gameOverTotalRingUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
        gameManager.onGameOver.AddListener(ActivateGameOverUI);
    }

    public void PlayBtnHandler() {
        gameManager.StartGame();
    }

    public void ActivateGameOverUI() {
        gameOverUI.SetActive(true);

        gameOverScoreUI.text = "Score: " + gameManager.ConvertToIntScore();
        gameOverHighscoreUI.text = "Highscore: " + gameManager.ConvertToIntHighScore();
        gameOverTotalRingUI.text = "Total ring: " + gameManager.GetCurrentRing();
    }

    private void OnGUI() {
        scoreUI.text = gameManager.ConvertToIntScore();
        ringUI.text = gameManager.GetCurrentRing();
    }
}
