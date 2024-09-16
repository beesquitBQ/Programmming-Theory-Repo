using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button menuButton;

    private int currentScore = 0;
    private bool isGameOver = false;

    private void Start()
    {
        InitializeUI();
        StartNewGame();
    }

    private void InitializeUI()
    {
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMenu);
        UpdateScoreText();
    }

    public void StartNewGame()
    {
        currentScore = 0;
        isGameOver = false;
        UpdateScoreText();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

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
