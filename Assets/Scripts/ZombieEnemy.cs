using UnityEngine;
public class ZombieEnemy : EnemyBase
{
    
    private void Awake()
    {
        maxHealth = 75f;
        speed = 0.5f;
        armor = Random.Range(50, 101);
        reward = 20;
        damageToBase = 10;

        originalSpeed = speed;
    }
}