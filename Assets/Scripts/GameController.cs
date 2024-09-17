using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ENCAPSULATION
public class GameController : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button menuButton;
    public TMP_Text healthText;
    private PlayerStats playerStats;

    private int currentScore = 0;
    private bool isGameOver = false;

    private void Start()
    {
        InitializeUI();
        StartNewGame();
        playerStats = FindObjectOfType<PlayerStats>();  // Находим PlayerStats в сцене
        if (playerStats != null)
        {
            playerStats.OnHealthChanged.AddListener(UpdateHealthText);  // Подписываемся на событие изменения здоровья
        }
    }

    // ABSTRACTION
    private void InitializeUI()
    {
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMenu);
        UpdateScoreText();
        UpdateHealthText(1f);
    }

    // ABSTRACTION
    public void StartNewGame()
    {
        currentScore = 0;
        isGameOver = false;
        UpdateScoreText();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    // ABSTRACTION
    public void IncreaseScore(int points)
    {
        if (!isGameOver)
        {
            currentScore += points;
            UpdateScoreText();
            Debug.Log($"Score increased by {points}. Current score: {currentScore}");
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{MainManager.Instance.playerName} : {currentScore}";
        }
    }

    private void UpdateHealthText(float healthPercentage)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(healthPercentage * 100)}%";
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        MainManager.Instance.AddScore(MainManager.Instance.playerName, currentScore);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
