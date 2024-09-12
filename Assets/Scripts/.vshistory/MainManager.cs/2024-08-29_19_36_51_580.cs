using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }
    public string playerName = "";
    public int bestScore;
    public string bestPlayerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighscore();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int bestScore;
        public string bestPlayerName;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData
        {
            playerName = playerName,
            bestPlayerName = bestPlayerName,
            bestScore = bestScore
        };
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(GetSaveFilePath(), json);
        Debug.Log($"Highscore saved: {bestPlayerName} - {bestScore}");
    }

    public void LoadHighscore()
    {
        string path = GetSaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestPlayerName = data.bestPlayerName;
            bestScore = data.bestScore;
            playerName = data.playerName;
            Debug.Log($"Highscore loaded: {bestPlayerName} - {bestScore}");
        }
        else
        {
            Debug.Log("No save file found. Starting with default values.");
        }
    }

    private string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    // Вызывайте этот метод перед выходом из игры
    private void OnApplicationQuit()
    {
        SaveHighscore();
    }
}
