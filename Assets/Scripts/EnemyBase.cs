using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    protected Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();

    protected Animator animator;
    protected const string MoveTriggerParam = "MoveTrigger";

    [Header("Enemy Stats")]
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public int armor = 0;

    [Header("Health Bar Sprites")]
    public SpriteRenderer fillSprite;       
    public SpriteRenderer backgroundSprite; 

    public float originalSpeed;
    private bool isSlowed = false;

    public int reward;
    public int damageToBase;

    protected Transform[] pathPoints;
    protected int currentPoint = 0;

    public virtual void Init(Transform[] points)
    {
        pathPoints = points;
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        originalColors.Clear();

        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr == fillSprite) continue;

            if (sr == backgroundSprite) continue;

            if (sr.sprite == null) continue;

            originalColors.Add(sr, sr.color);
        }

        UpdateHealthBar();
    }

    private void Update()
    {
        Move();
    }

    protected void Move()
    {
        if (currentPoint >= pathPoints.Length)
        {
            ReachBase();
            return;
        }

        if (animator != null && speed > 0)
        {
            animator.SetTrigger(MoveTriggerParam);
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            pathPoints[currentPoint].position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, pathPoints[currentPoint].position) < 1.0f)
        {
            currentPoint++;
        }
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        StartCoroutine(HitFlashCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private IEnumerator HitFlashCoroutine()
    {
        Color flashColor = isSlowed ? Color.magenta : Color.red;

        foreach (var pair in originalColors)
        {
            if (pair.Key != null) pair.Key.color = flashColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var pair in originalColors)
        {
            if (pair.Key != null)
            {
                if (isSlowed) pair.Key.color = Color.blue;
                else pair.Key.color = pair.Value;
            }
        }
    }

    public void ApplySlowEffect(float slowPercentage, float duration)
    {
        StopCoroutine("SlowCoroutine");
        StartCoroutine(SlowCoroutine(slowPercentage, duration));
    }

    private IEnumerator SlowCoroutine(float slowPercentage, float duration)
    {
        isSlowed = true;
        speed = originalSpeed * (1f - slowPercentage);

        foreach (var pair in originalColors)
        {
            if (pair.Key != null) pair.Key.color = Color.blue;
        }

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        isSlowed = false;

        foreach (var pair in originalColors)
        {
            if (pair.Key != null) pair.Key.color = pair.Value;
        }
    }

    public void UpdateHealthBar()
    {
        if (fillSprite != null)
        {
            float healthRatio = currentHealth / maxHealth;
            fillSprite.transform.localScale = new Vector3(healthRatio, fillSprite.transform.localScale.y, 1f);

            if (healthRatio < 0.3f) fillSprite.color = Color.red;
            else fillSprite.color = Color.green;
        }
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddGold(reward);
        if (SpawnManager.Instance != null) SpawnManager.Instance.EnemyDefeated();
        Destroy(gameObject);
    }

    protected virtual void ReachBase()
    {
        GameManager.Instance.TakeDamage(damageToBase);
        SpawnManager.Instance.EnemyDefeated();
        Destroy(gameObject);
    }
}