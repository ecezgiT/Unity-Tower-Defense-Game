using UnityEngine;

public class CannonTower : TowerBase
{
    public float splashRange = 50f;

    private void Awake()
    {
        towerName = "TopcuKulesi";
        cost = 75;
        baseDamage = 20f;
        fireRate = 3.0f;
        range = 7.5f;
    }

    public override bool IsTargetValid(EnemyBase enemy)
    {
        return !(enemy is GhostEnemy);
    }

    protected override void Attack(EnemyBase mainTarget)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("CannonTower: Projectile Prefab is missing!");
            return;
        }

        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;

        spawnPos.z = 0f;

        GameObject projGO = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Projectile projScript = projGO.GetComponent<Projectile>();
        if (projScript != null)
        {
            float netDamage = CalculateDamage(mainTarget.armor, baseDamage);
            projScript.Setup(mainTarget, netDamage, this);
        }
    }
}