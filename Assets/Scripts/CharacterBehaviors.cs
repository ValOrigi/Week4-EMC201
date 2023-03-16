using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviors : MonoBehaviour
{
    public float walkSpeed;
    public bool facingRight = true;
    public float inputHorizontal;

    public bool canDoubleJump;
    public float offset = 0.2f;
    public float jumpPower;

    public Animator anim;
    public enum MovementState { idle, walking, running, jumping }
    public MovementState state = MovementState.idle;

    public static Vector2 respawnPosition = new Vector2(-10f, -3.25f);

    Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject.FindGameObjectWithTag("Player").transform.position = CharacterBehaviors.respawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && rigidBody2D.velocity.y == 0f)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0f);
            rigidBody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
            {
                canDoubleJump = false;
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0f);
                rigidBody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
        }

        Turn();

        walkSpeed = 5f;
        if (Input.GetKey("left shift"))
        {
            walkSpeed *= 2f;
        }

        UpdateAnimationState();
    }

    void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(inputHorizontal * walkSpeed, rigidBody2D.velocity.y);
    }

    void Turn()
    {
        if ((facingRight && inputHorizontal < 0f) || (!facingRight && inputHorizontal > 0f))
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);

        }
    }

    void UpdateAnimationState()
    {
        MovementState state;
        if (walkSpeed > 5f && inputHorizontal != 0f)
        {
            state = MovementState.running;
        }
        else if (walkSpeed == 5f && inputHorizontal != 0f)
        {
            state = MovementState.walking;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rigidBody2D.velocity.y != 0f)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }
}
