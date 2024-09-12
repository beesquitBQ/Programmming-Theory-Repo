using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; };

    public TMP_Text ScoreText;
    public GameObject GameOverPanel;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

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
            GameOverPanel.SetActive(false);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void StartGame()
    {
        m_Started = true;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        UpdateScoreText();
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverPanel.SetActive(true);
        MainManager.Instance.AddScore(MainManager.Instance.playerName , m_Points);
    }

    private void UpdateScoreText()
    {
        ScoreText.text = $"Score : {MainManager.Instance.playerName} : {m_Points}";
    }
}
