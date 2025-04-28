using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
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
    }

    private void OnGUI() {
        scoreUI.text = gameManager.Score();
    }
}
