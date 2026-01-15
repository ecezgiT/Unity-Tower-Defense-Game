using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject projectilePrefab;  
    public Transform firePoint;         

    [Header("Tower Stats")]
    public string towerName;
    public int cost;
    public float range;
    public float baseDamage;
    public float fireRate;

    protected float nextFireTime;
    protected GameManager gameManager;
    protected Transform baseTarget;

    private void Start()
    {
        gameManager = GameManager.Instance;

        GameObject baseObject = GameObject.FindWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
        }

        InvokeRepeating(nameof(CheckForTargets), 0f, 0.5f);
    }

    protected virtual EnemyBase FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        EnemyBase bestTarget = null;
        float minDistanceToBase = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            EnemyBase enemy = collider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                if (IsTargetValid(enemy))
                {
                    if (baseTarget != null)
                    {
                        float distanceToBase = Vector3.Distance(enemy.transform.position, baseTarget.position);

                        if (distanceToBase < minDistanceToBase)
                        {
                            minDistanceToBase = distanceToBase;
                            bestTarget = enemy;
                        }
                    }
                    else
                    {
                        bestTarget = enemy;
                    }
                }
            }
        }
        return bestTarget;
    }

    public virtual bool IsTargetValid(EnemyBase enemy)
    {
        return true;
    }

    protected void CheckForTargets()
    {
        if (Time.time >= nextFireTime)
        {
            EnemyBase target = FindTarget();
            if (target != null)
            {
                Attack(target);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    protected abstract void Attack(EnemyBase target);

    public float CalculateDamage(int enemyArmor, float towerDamage)
    {
        float netDamage = towerDamage * (1f - (enemyArmor / (enemyArmor + 100.0f)));
        return netDamage;
    }

}