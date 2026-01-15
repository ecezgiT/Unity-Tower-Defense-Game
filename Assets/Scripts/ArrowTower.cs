using UnityEngine;

public class ArrowTower : TowerBase
{
    private const float ARMOR_PENALTY_MULTIPLIER = 0.5f;

    [Header("Character Visuals")]
    public Animator characterAnimator;

    private void Awake()
    {
        towerName = "OkcuKulesi";
        cost = 50;
        baseDamage = 10f;
        fireRate = 1.0f;
        range = 7f;
    }

    protected override void Attack(EnemyBase target)
    {
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Shoot");
        }

        float damageToPass = baseDamage;
        if (target is ZombieEnemy)
        {
            damageToPass *= ARMOR_PENALTY_MULTIPLIER;
        }
        float finalNetDamage = CalculateDamage(target.armor, damageToPass);

        if (projectilePrefab != null)
        {
            Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;
            spawnPos.z = 0f;

            GameObject projGO = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            Projectile projScript = projGO.GetComponent<Projectile>();

            if (projScript != null)
            {
                projScript.Setup(target, finalNetDamage, this);
            }
        }
        else
        {
            Debug.LogError("ArrowTower: Projectile Prefab is missing!");
        }
    }
}