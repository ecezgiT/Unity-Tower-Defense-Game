using UnityEngine;

public class SkeletonEnemy : EnemyBase
{
  
    private void Awake()
    {
        maxHealth = 50f;
        speed = 1f;
        armor = 0;
        reward = 10;
        damageToBase = 5;
        originalSpeed = speed;
    }
}