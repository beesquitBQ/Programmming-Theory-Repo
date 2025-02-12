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
    public TMP_Text healthText;
    private PlayerStats playerStats;
    private int currentScore = 0;
    private bool isGameOver = false;
    [SerializeField] private GameObject pauseMenuPanel; // Ссылка на панель меню паузы
    private bool isGamePaused = false;

    private void Awake()
    {
        isGamePaused = false;
    }

    private void Start()
    {
        InitializeUI();
        StartNewGame();
        playerStats = FindObjectOfType<PlayerStats>();  // Находим PlayerStats в сцене
        if (playerStats != null)
        {
            playerStats.OnHealthChanged.AddListener(UpdateHealthText);  // Подписываемся на событие изменения здоровья
        }
        HideCursor();
        isGamePaused = false;
    }

    private void Update()
    {
        // Проверяем нажатие клавиши Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
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
        // Сбрасываем время при старте новой игры
        Time.timeScale = 1f;

        currentScore = 0;
        isGameOver = false;
        UpdateScoreText();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        isGamePaused = false;
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
        isGamePaused = true;
        isGameOver = true;
        ShowCursor();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        MainManager.Instance.AddScore(MainManager.Instance.playerName, currentScore);
    }

    public void RestartGame()
    {
        // Сбрасываем время перед перезапуском игры
        Time.timeScale = 1f;

        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        // Сбрасываем время перед возвратом в главное меню
        Time.timeScale = 1f;

        isGamePaused = false;
        SceneManager.LoadScene(0);
    }

    // Метод для переключения паузы
    public void TogglePause()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // Скрываем меню паузы
        Time.timeScale = 1f; // Возобновляем время
        HideCursor(); // Скрываем курсор
        isGamePaused = false;
    }

    // Метод для паузы игры
    private void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // Показываем меню паузы
        Time.timeScale = 0f; // Останавливаем время
        ShowCursor(); // Показываем курсор
        isGamePaused = true;
    }

    // Метод для скрытия курсора
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Метод для показа курсора
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Application.Quit(); // Закрывает приложение (работает только в билде, а не в редакторе)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Для тестирования в редакторе
#endif
    }
}