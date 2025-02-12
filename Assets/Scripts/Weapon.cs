using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDamage = 8.0f;
    [SerializeField] protected float minDamage = 4.0f;
    [SerializeField] protected float reduceDamageMultiplier = 0.5f;
    [SerializeField] protected float attackDuration = 0.5f;
    [SerializeField] protected AudioMixer audioMixer;
    public float damageMultiplier = 1.5f;
    public float scoreMultiplier = 1.25f;

    // Звуки
    [SerializeField] protected AudioClip swingSound; // Звук взмаха
    protected abstract AudioClip GetSpecialHitSound(Enemy enemy);
    protected abstract AudioClip GetRegularHitSound();

    protected PlayerStats playerStats;
    protected bool isBlocking = false;
    protected bool isAttacking = false;
    protected Animator weaponAnimator;
    protected Collider weaponCollider;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        weaponAnimator = GetComponent<Animator>();
        weaponCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>(); // Получаем AudioSource
        InitializeCollider();
    }

    protected virtual void Update()
    {
        isBlocking = Input.GetMouseButton(1);
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    protected virtual IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Воспроизводим звук взмаха
        PlaySound(swingSound);

        weaponAnimator.SetTrigger("Attack");
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isAttacking)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = Random.Range(minDamage, maxDamage);

                // Проверяем, является ли противник специализированным
                bool isSpecialized = IsSpecializedEnemy(enemy);

                ApplyDamageToEnemy(enemy, damage);

                // Воспроизводим звук удара
                if (isSpecialized)
                {
                    PlaySound(GetSpecialHitSound(enemy));
                }
                else
                {
                    PlaySound(GetRegularHitSound());
                }
            }
        }
    }

    // Метод для проверки, является ли противник специализированным
    protected virtual bool IsSpecializedEnemy(Enemy enemy)
    {
        return enemy is Slime || enemy is Skeleton; // Пример: слаймы и скелеты считаются специализированными
    }

    protected abstract void ApplyDamageToEnemy(Enemy enemy, float damage);

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (isBlocking)
        {
            damageAmount *= reduceDamageMultiplier;
        }
        playerStats.TakeDamage(damageAmount);
    }

    protected virtual void InitializeCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            weaponCollider.isTrigger = true;
        }
    }

    // Метод для воспроизведения звука
    protected void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Создаем временный AudioSource для воспроизведения звука через AudioMixer
            GameObject tempAudioObject = new GameObject("TempAudio");
            AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();
            tempAudioSource.clip = clip;
            tempAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; // Группа SFX
            tempAudioSource.Play();

            // Уничтожаем объект после завершения воспроизведения
            Destroy(tempAudioObject, clip.length);
        }
    }

}