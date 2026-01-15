using UnityEngine;

public class FrostTower : TowerBase
{
    private void Awake()
    {
       
        towerName = "BuzKulesi";
        cost = 70;
        baseDamage = 15f;
        fireRate = 2.0f;
        range = 5f;
    }

    protected override void Attack(EnemyBase target)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("FrostTower: Projectile Prefab is missing!");
            return;
        }
        float netDamage = CalculateDamage(target.armor, baseDamage);

        Vector3 spawnPos = (firePoint != null) ? firePoint.position : transform.position;

        spawnPos.z = 0f;

         GameObject projGO = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projScript = projGO.GetComponent<Projectile>();

        if (projScript != null)
        {
             projScript.Setup(target, netDamage, this);
        }
    }
}