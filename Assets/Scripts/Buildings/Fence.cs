using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fence : GridEntity
{


    public ushort Radius;
    public ushort Damage;
    public ushort FireRate;
    public string AttackAnimationName;
    public ushort SelfDamageRate;

    [HideInInspector] public CircleCollider2D Collider2D;
    [HideInInspector] public GameObject GameObject; 
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public Vector2Int Position;



    public void DamageEnemiesInRadius()
    {

        List<Transform> enemiesInRange = new List<Transform>();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(GameObject.transform.position, Radius);
        foreach (Collider2D enemy in enemies)
            if (enemy.tag == "Enemy")
                enemiesInRange.Add(enemy.transform);
         
        foreach (Transform enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(GameObject.transform.position, enemy.position);
            if (distanceToEnemy < Radius)
            {
                //DAMAGE ENEMY HERE
            }
        } 
    }

    public override void FixedUpdate()
    {   
        
    }   
}
