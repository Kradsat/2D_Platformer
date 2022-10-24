using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour  
{
    //start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    

    public int cherries = 0;

    //FSM (Finite State Machine)
    private enum State { idle, running, jumping, falling, hurt}
    private State state = State.idle;


    //inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10.0f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    
    }

    private void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }
        
        AnimationState();
        //FootSteps();
        anim.SetInteger("state", (int)state);           // set animation based on enum state

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
           
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (state == State.falling)
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.JumpOn();
                //Destroy(other.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is to the right, therefore player be damage and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //enemy is to the left, therefore player be damage and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
            
        }
    }

    private void Movement()
    {
        float h_Direction = Input.GetAxis("Horizontal");
       
        //MOVE LEFT
        if (h_Direction < 0)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //MOVE RIGHT
        else if (h_Direction > 0)
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        else
        {
        }


        //JUMPING
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }

        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) >0.1f)
        {
            //MOVING
            state = State.running;
        }
        else
        {
            state = State.idle;
        }


    }

    private void FootSteps()
    {
        footstep.Play();
    }
}
