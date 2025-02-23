using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType{
        Basic,
        Medium,
        Hard
    }

    public EnemyType enemyType;
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            OnDeath();
        }
    }

    // Those methods are coming from Wconomy Score Manager Script
    public void OnDeath(){
        switch (enemyType)
        {
            case EnemyType.Basic:
                EconomyScoreManager.Instance.OnBasicEnemyDefeated(transform.position);
                break;

            case EnemyType.Medium:
                EconomyScoreManager.Instance.OnMediumEnemyDefeated(transform.position);
                break;

            case EnemyType.Hard:
                EconomyScoreManager.Instance.OnHardEnemyDefeated(transform.position);
                break;
        }
        Destroy(gameObject);
    }


}
