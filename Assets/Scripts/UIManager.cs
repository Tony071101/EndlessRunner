using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameManager.Instance;
    }

    private void OnGUI() {
        scoreUI.text = gameManager.Score();
    }
}
