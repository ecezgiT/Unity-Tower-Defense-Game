using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;

    private EnemyBase target;
    private Collider2D targetCollider;
    private float damage;
    private TowerBase sourceTower;
    private float splashRange = 0f;

    public void Setup(EnemyBase enemy, float dmg, TowerBase source)
    {
        target = enemy;
        damage = dmg;
        sourceTower = source;

        targetCollider = enemy.GetComponent<Collider2D>();

        if (source is CannonTower cannonTower)
        {
            splashRange = cannonTower.splashRange;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = (targetCollider != null) ? targetCollider.bounds.center : target.transform.position;

        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
        }
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target == null) return;

        target.TakeDamage(damage);

        string effectInfo = "";
        if (sourceTower is FrostTower)
        {
            target.ApplySlowEffect(0.5f, 3f);
            effectInfo = ", Yavaşlatma %50 (3 sn) uygulandı";
        }

        if (LogManager.Instance != null)
        {
            
            string logMessage = $"Kule '{sourceTower.towerName}', '{target.name}'i hedefledi; Net Hasar {damage:F2}{effectInfo}. Kalan Can: {target.currentHealth:F2}/{target.maxHealth:F2}.";

            LogManager.Instance.WriteLog(logMessage);
        }

       
        if (splashRange > 0)
        {
            ApplySplashDamage(target.transform.position);
        }

        Destroy(gameObject);
    }

    private void ApplySplashDamage(Vector3 center)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, splashRange);
        CannonTower cannonTower = sourceTower as CannonTower;

        if (cannonTower == null) return;

        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyBase splashTarget = hitCollider.GetComponent<EnemyBase>();

            if (splashTarget != null && splashTarget != target)
            {
                if (cannonTower.IsTargetValid(splashTarget))
                {
                    float splashDamage = cannonTower.CalculateDamage(splashTarget.armor, cannonTower.baseDamage);
                    splashTarget.TakeDamage(splashDamage);

                    if (LogManager.Instance != null)
                    {
                       
                        string logMessage = $"Kule '{cannonTower.towerName}', yakındaki '{splashTarget.name}'i hedefledi (Alan Hasarı); Net Hasar {splashDamage:F2}.";

                        LogManager.Instance.WriteLog(logMessage);
                    }
                }
            }
        }
    }
}