using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI ringUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;
    [SerializeField] private TextMeshProUGUI gameOverTotalRingUI;
    [SerializeField] private TextMeshProUGUI shopTotalRingUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject ShopUI;
    [SerializeField] private GameObject characterItemPrefab;
    [SerializeField] private Transform itemContainer;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
        gameManager.onGameOver.AddListener(ActivateGameOverUI);

        StartCoroutine(WaitAndBuildUI());
    }

    private IEnumerator WaitAndBuildUI()
    {
        yield return new WaitUntil(() => CharacterManager.Instance.saveDataList != null);
        BuildCharacterUI();
    }

    private void BuildCharacterUI()
    {
        var infos = CharacterManager.Instance.GetAllCharacters();
        var saves = CharacterManager.Instance.saveDataList;

        foreach(var info in infos) {
            var save = saves.Find(s => s.characterId == info.characterId);
            if(save == null) {
                continue;
            }  
            var item = Instantiate(characterItemPrefab, itemContainer);
            var ui = item.GetComponent<CharacterItemUI>();
            ui.SetUp(info, save);
        }
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

    public void ActivateShopUI() {
        ShopUI.SetActive(true);
    }

    private void OnGUI() {
        scoreUI.text = gameManager.ConvertToIntScore();
        ringUI.text = gameManager.GetCurrentRing();
        shopTotalRingUI.text = "Total ring: " + gameManager.GetTotalRings();
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;  // Dừng chạy trong Editor
        #else
        Application.Quit();  // Thoát ứng dụng trong build thực tế
        #endif
    }

}
