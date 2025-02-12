using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel; // Ссылка на панель меню паузы

    private bool isGamePaused = false;

    private void Start()
    {
        // Изначально скрываем курсор при старте игры
        HideCursor();
    }

    private void Update()
    {
        // Проверяем нажатие клавиши Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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
    private void ResumeGame()
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