using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Linq;

public class MainManager : MonoBehaviour
{
    // ENCAPSULATION
    public static MainManager Instance { get; private set; }
    public string playerName = "";
    public List<PlayerScore> leaderList = new List<PlayerScore>();
    private const int MaxLeaderListSize = 10;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeaderList();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;

        public PlayerScore(string name, int score)
        {
            this.playerName = name;
            this.score = score;
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public List<PlayerScore> leaderList;
    }

    // ABSTRACTION
    public void AddScore(string playerName, int score)
    {
        var existingScore = leaderList.FirstOrDefault(p => p.playerName == playerName);
        if (existingScore != null)
        {
            if (score > existingScore.score)
            {
                existingScore.score = score;
            }
        }
        else
        {
            leaderList.Add(new PlayerScore(playerName, score));
        }
        leaderList = leaderList.OrderByDescending(p => p.score).Take(MaxLeaderListSize).ToList();
        SaveLeaderList();
    }

    // ABSTRACTION
    public void SaveLeaderList()
    {
        SaveData data = new SaveData
        {
            leaderList = leaderList
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetSaveFilePath(), json);
    }

    // ABSTRACTION
    public void LoadLeaderList()
    {
        string path = GetSaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            leaderList = data.leaderList;
        }
        else
        {
            leaderList = new List<PlayerScore>();
        }
    }

    private string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    private void OnApplicationQuit()
    {
        SaveLeaderList();
    }
}
