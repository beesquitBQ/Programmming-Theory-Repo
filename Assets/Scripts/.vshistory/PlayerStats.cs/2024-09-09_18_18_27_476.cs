using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : Creature
{
    public int currentScore = 0;
    private Text scoreText;

    protected override void Start()
    {
        maxHealth = 100f;
        base.Start();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
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
