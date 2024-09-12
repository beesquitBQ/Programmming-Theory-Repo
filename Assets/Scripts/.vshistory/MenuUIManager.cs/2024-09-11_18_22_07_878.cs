using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(0)]
public class MenuUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public GameObject scoresPanel;
    public TMP_Text leaderboardText;

    private void Start()
    {
        if (MainManager.Instance == null)
        {
            Debug.LogWarning("MainManager is not initialized. Creating a new instance.");
            GameObject mainManagerObject = new GameObject("MainManager");
            mainManagerObject.AddComponent<MainManager>();
        }

        if (playerNameInput != null && !string.IsNullOrEmpty(MainManager.Instance.playerName))
        {
            playerNameInput.text = MainManager.Instance.playerName;
        }
    }

    public void SetPlayerName()
    {
        if (MainManager.Instance != null && playerNameInput != null)
        {
            MainManager.Instance.playerName = playerNameInput.text;
            MainManager.Instance.SaveLeaderList();
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

    public void OpenScoresPanel()
    {
        scoresPanel.SetActive(true);
        UpdateLeaderboardDisplay();
    }

    public void CloseScoresPanel()
    {
        scoresPanel.SetActive(false);
    }

    public void UpdateLeaderboardDisplay()
    {
        if (leaderboardText != null)
        {
            string leaderboardString = "Leaderboard:\n";
            for (int i = 0; i < MainManager.Instance.leaderList.Count; i++)
            {
                var playerScore = MainManager.Instance.leaderList[i];
                leaderboardString += $"{i + 1}. {playerScore.playerName}: {playerScore.score}\n";
            }
            leaderboardText.text = leaderboardString;
        }
    }

    public void Exit()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.SaveLeaderList();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}



