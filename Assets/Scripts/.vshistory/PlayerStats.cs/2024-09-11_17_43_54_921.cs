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

        private PlayerMovement playerMovement;
    private PlayerLook playerLook;

    protected override void Start()
    {
        maxHealth = 30f;
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

    public override void Die()
    {
        if (!isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
            if (deathAnimator != null)
            {
                deathAnimator.SetTrigger("Die");
            }
            Debug.Log($"{gameObject.name} has been defeated!");
            StartCoroutine(PlayerDeathCoroutine());
        }
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        if (deathAnimator != null)
        {
            yield return new WaitForSeconds(deathAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.GameOver();

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        this.enabled = false;
    }

    protected override IEnumerator DeathCoroutine()
    {
        yield return base.DeathCoroutine();
    }


}
