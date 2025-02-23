using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager GameManager;
    public Animator Animator;
    public Rigidbody2D Rigid;
    public Vector2 TargetPosition;
    public float Speed;
    void Start()
    {
        
    } 
    public void SetTargetPosition(Vector2 Position)
    {
        TargetPosition = Position;
        this.Rigid.velocity = Vector2.zero;

        Vector2 Difference = TargetPosition - (Vector2)this.transform.position;

        if (Mathf.Abs(Difference.y)> Mathf.Abs(Difference.x))
        {
            if (Difference.y>0)
                Animator.Play("WalkUp");
            else
                Animator.Play("WalkDown");
        }
        else if (Mathf.Abs(Difference.y) < Mathf.Abs(Difference.x))
        { 
            if (Difference.x > 0)
                Animator.Play("WalkRight");
            else
                Animator.Play("WalkLeft");
        } 
    }


    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {


        this.Rigid.AddForce((TargetPosition - (Vector2)this.transform.position).normalized*1);
        if (Vector2.Distance((Vector2)this.transform.position, TargetPosition) < 0.2f)
        {
            this.Rigid.velocity = Vector2.zero;
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("WalkUp"))
            {
                Animator.Play("Idle Up");
            }
            else if (Animator.GetCurrentAnimatorStateInfo(0).IsName("WalkDown"))
            { 
                Animator.Play("Idle Down");
            }
            else if (Animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRight"))
            {

                Animator.Play("Idle Right");
            }
            else if (Animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLeft"))
            {

                Animator.Play("Idle Left");
            }
        }


        Rigid.velocity = Rigid.velocity.normalized * Speed;

        //print("Distance: " + Vector2.Distance((Vector2)this.transform.position, TargetPosition));

    }
}
