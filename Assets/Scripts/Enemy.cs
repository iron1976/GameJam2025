using UnityEngine;
using System.Collections;

[System.Serializable]
public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public Animator Animator;
    public Rigidbody2D _rigidbody2D;


    [Header("Enemy Configuration")]
    public EnemyType enemyType;
    public float maxHealth = 100f;
    public float currentHealth;
    public float movementSpeed = 2f;
    public float damage = 10f;
    public float attackRange = 1f;
    public float attackRate = 1f;

    [Header("Movement")]
    public float detectionRadius = 5f;
    public bool canMoveHorizontally = true;

    [Header("Components")]
    [SerializeField] private Transform healthBarTransform;
    [SerializeField] private SpriteRenderer healthBarRenderer;
    
    // Internal state
    private Transform currentTarget;
    private float lastAttackTime;
    private bool isNightTime;

    public enum EnemyType
    {
        Basic,
        Medium,
        Hard
    }

    private void Start()
    {
        currentHealth = maxHealth;
        InitializeHealthBar();
        StartCoroutine(FindTargetRoutine());
    }

    private void InitializeHealthBar()
    {
        // Setup health bar position and initial scale
        if (healthBarTransform != null)
        {
            healthBarTransform.localPosition = new Vector3(0, -0.5f, 0);
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarTransform != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBarTransform.localScale = new Vector3(healthPercentage, 0.1f, 1f);
            healthBarRenderer.color = Color.Lerp(Color.red, Color.green, healthPercentage);
        }
    }

    private void Update()
    {
        Move();
        TryAttack();
    }

    private void Move()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            
            // If canMoveHorizontally is true, maintain Y position
            if (canMoveHorizontally)
            {
                direction.y = 0;
            }
            
            transform.position += direction * movementSpeed * Time.deltaTime;
            Animator.Play("Walk");
        }
    }

    private IEnumerator FindTargetRoutine()
    {
        while (true)
        {
            FindNearestTarget();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FindNearestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tower") || collider.CompareTag("CampFire"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = collider.transform;
                }
            }
        }

        currentTarget = closestTarget;
    }

    private void TryAttack()
    {
        if (currentTarget != null && Time.time >= lastAttackTime + attackRate)
        {
            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
            
            if (distanceToTarget <= attackRange)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        // Get the component that can take damage
        GridEntity targetEntity = currentTarget.GetComponent<GridEntity>();
        if (targetEntity != null)
        {
            targetEntity.GetDamaged((ushort)damage);
            Animator.Play("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
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