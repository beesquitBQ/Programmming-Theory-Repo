using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMP_Text ScoreText;
    public GameObject GameOverPanel;
    public Button MenuButton;
    public Button RestartButton;

    private bool m_Started = false;
    private int m_Score;
    private bool m_GameOver = false;

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
        GameOverPanel.SetActive(false);
        if (RestartButton != null)
            RestartButton.onClick.AddListener(RestartGame);
        if (MenuButton != null)
            MenuButton.onClick.AddListener(ReturnToMenu);
    }

    void Update()
    {
        //if (!m_Started)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        StartGame();
        //    }
        //}
        //else if (m_GameOver)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        RestartGame();
        //    }
        //}
    }

    void StartGame()
    {
        m_Started = true;
        m_GameOver = false;
        m_Score = 0;
        UpdateScoreText();
        GameOverPanel.SetActive(false);
    }

    public void IncreaseScore(int points)
    {
        if (!m_GameOver)
        {
            m_Score += points;
            OnScoreChanged?.Invoke(m_Score);
            UpdateScoreText();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverPanel.SetActive(true);
        MainManager.Instance.AddScore(MainManager.Instance.playerName, m_Score);
    }

    private void UpdateScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"{MainManager.Instance.playerName} : {m_Score}";
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetupUI(TMP_Text scoreText, GameObject gameOverPanel, Button restartButton, Button menuButton)
    {
        ScoreText = scoreText;
        GameOverPanel = gameOverPanel;
        RestartButton = restartButton;
        MenuButton = menuButton;
        if (RestartButton != null)
            RestartButton.onClick.AddListener(RestartGame);
        if (MenuButton != null)
            MenuButton.onClick.AddListener(ReturnToMenu);
        UpdateScoreText();
        GameOverPanel.SetActive(false);
    }
}


