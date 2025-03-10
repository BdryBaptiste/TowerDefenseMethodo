using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public Button startButton;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI goldText;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        GameManager.Instance.OnLivesChanged += UpdateLivesUI;
        UpdateLivesUI(GameManager.Instance.GetLives());
        GameManager.Instance.OnGoldChanged += UpdateGoldUI;
        UpdateGoldUI(GameManager.Instance.GetGold());
    }

    private void StartGame()
    {
        gameManager.StartGame();
        startButton.gameObject.SetActive(false); // Hide button after starting
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLivesChanged -= UpdateLivesUI; // Remove listener when destroyed
        GameManager.Instance.OnGoldChanged -= UpdateGoldUI; // Remove listener when destroyed
    }

    private void UpdateLivesUI(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    private void UpdateGoldUI(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }
}
