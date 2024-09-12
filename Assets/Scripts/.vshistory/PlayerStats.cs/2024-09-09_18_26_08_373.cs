using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class PlayerStats : Creature
{
    public int currentScore = 0;
    private TMP_Text scoreText;
    public event Action<int> OnScoreChanged;

    protected override void Start()
    {
        maxHealth = 100f;
        base.Start();
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        OnScoreChanged += UpdateScoreUI;
    }

    public void IncreaseScore(int score)
    {
        currentScore += score;
        UpdateScoreUI(currentScore);
    }

    private void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
