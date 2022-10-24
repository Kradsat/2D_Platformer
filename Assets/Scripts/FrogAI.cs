using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAI : Enemy
{
    private Collider2D coll;
  
    [SerializeField] private LayerMask ground;

    [SerializeField] private float leftCap;
    [SerializeField] private float rigthCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 10f;

    private bool facingleft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();

    }

    private void Update()
    {
        //transition from jump to fall
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0.1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //transition from fall to idle
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }

    }

    private void Move()
    {
        if (facingleft)
        {

            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector3(1, 1);
            }
            //test to see if frog is beyond the leftcap
            if (transform.position.x > leftCap)
            {
                
                //see if it is on the ground, if so, jump
                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingleft = false;
            }
            //if not, is going to face right
        }
        else
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector3(-1, 1);
            }
            if (transform.position.x < rigthCap)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingleft = true;
            }
        }
    }

   
}

   
