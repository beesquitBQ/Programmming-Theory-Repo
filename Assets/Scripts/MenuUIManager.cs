using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
            string leaderboardString = "Leader Board:\n";
            var sortedList = MainManager.Instance.leaderList.OrderByDescending(p => p.score).Take(10);
            int rank = 1;
            foreach (var playerScore in sortedList)
            {
                leaderboardString += $"{rank}. {playerScore.playerName}: {playerScore.score}\n";
                rank++;
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



