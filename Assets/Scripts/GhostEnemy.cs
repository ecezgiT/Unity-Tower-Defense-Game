using UnityEngine;

public class GhostEnemy : EnemyBase
{
   

    private void Awake()
    {
        maxHealth = 50f;
        speed = 1.5f;
        armor = 0;
        reward = 15;
        damageToBase = 5;

        originalSpeed = speed;
    }
}
