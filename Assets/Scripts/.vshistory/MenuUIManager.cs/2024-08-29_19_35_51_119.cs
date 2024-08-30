using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance == null)
        {
            Debug.LogWarning("MainManager is not initialized. Creating a new instance.");
            GameObject mainManagerObject = new GameObject("MainManager");
            mainManagerObject.AddComponent<MainManager>();
        }

        if (playerNameInput != null)
        {
            if (!string.IsNullOrEmpty(MainManager.Instance.playerName))
            {
                playerNameInput.text = MainManager.Instance.playerName;
            }
        }
        else
        {
            Debug.LogError("playerNameInput is not assigned in MenuUIManager");
        }
    }

    public void SetPlayerName()
    {
        if (MainManager.Instance != null && playerNameInput != null)
        {
            MainManager.Instance.playerName = playerNameInput.text;
            MainManager.Instance.SaveHighscore();  // Сохраняем изменения
            Debug.Log($"Player name set and saved: {MainManager.Instance.playerName}");
        }
        else
        {
            Debug.LogError("MainManager is not initialized or playerNameInput is null in SetPlayerName()");
        }
    }

    public void StartNew()
    {
        SetPlayerName();
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.SaveHighscore();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

