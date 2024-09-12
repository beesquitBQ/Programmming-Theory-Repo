using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class PlayerStats : Creature
{
    public int currentScore = 0;
    public TMP_Text scoreText;
    public event Action<int> OnScoreChanged;

    protected override void Start()
    {
        maxHealth = 100f;
        base.Start();
        InitializeScoreText();
        OnScoreChanged += UpdateScoreUI;
    }

    private void InitializeScoreText()
    {
        GameObject scoreTextObject = GameObject.Find("ScoreText");
        if (scoreTextObject != null )
        {
            scoreText = scoreTextObject.GetComponent<TMP_Text>();
        }
    }

    public void IncreaseScore(int score)
    {
        currentScore += score;
        OnScoreChanged?.Invoke(currentScore);
    }

    private void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth/maxHealth);
        Debug.Log($"{gameObject.name} healed for {amount}. Current health {currentHealth}");
    }
}
