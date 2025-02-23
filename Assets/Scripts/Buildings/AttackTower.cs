using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackTower : GridEntity
{
    public float Radius;
    public ushort Damage;
    public ushort FireRate;
    public string AttackAnimationName;
    public AttackTypeEnum AttackType;
    public enum AttackTypeEnum
    {
        Direct = 1,
        Radius = 2
    }



    [HideInInspector] public CircleCollider2D Collider2D; 
    [HideInInspector] public Transform TargetEnemy;
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public Vector2Int Position;
    [HideInInspector] public Action AttackLogic = () => { };


    public void UpdateTarget()
    {

        List<Transform> enemiesInRange = new List<Transform>();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(this.GridSpriteRenderer.transform.position, Radius);
        foreach (Collider2D enemy in enemies)
            if (enemy.tag == "Enemy")
                enemiesInRange.Add(enemy.transform);

        float shortestDistance = 9999;
        Transform nearestEnemy = null;
        foreach (Transform enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(this.GridSpriteRenderer.transform.position, enemy.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        TargetEnemy = nearestEnemy;
    }
    public void DirectAttackLogic()
    { 
        attackTimer++; 
        if (attackTimer >= FireRate)
        {
            UpdateTarget();
            if (TargetEnemy != null)
            {
                //DAMAGE ENEMY
                attackTimer = 0;
            }
        }
    }
    public void RadiusAttackLogic()
    { 
        attackTimer++; 
        if (attackTimer >= FireRate)
        { 
            Collider2D[] enemies = Physics2D.OverlapCircleAll(this.GridSpriteRenderer.transform.position, Radius); 
             
            if (enemies != null && enemies.Length > 0)
            {
                //DAMAGE ENEMY
                attackTimer = 0;
            }
        }
    }


    public override void Start()
    {
        
        if (this.AttackType == AttackTypeEnum.Direct)
            AttackLogic = DirectAttackLogic;

        else if (this.AttackType == AttackTypeEnum.Radius)
            AttackLogic = RadiusAttackLogic;
    }
    ushort attackTimer = 0;
    public override void FixedUpdate()
    {
        AttackLogic();
    }
    public override void Update()
    { 
    }
}
