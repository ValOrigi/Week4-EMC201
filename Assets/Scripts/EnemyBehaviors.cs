using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviors : MonoBehaviour
{
    public float walkSpeed;
    public float tempWalkSpeed;

    [SerializeField] LayerMask target;
    Rigidbody2D rigidBody2D;

    public bool facingRight = true;
    public bool shotCooldown = false;
    public bool shot = false;

    public Animator anim;
    public enum MovementState { walking, charging }
    public MovementState state = MovementState.walking;

    public GameObject bulletPrefab;
    public Transform projOffset;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        rigidBody2D.velocity = new Vector2(walkSpeed, 0f);

        RaycastHit2D hit;

        if (facingRight)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right, 10f, target);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left, 10f, target);
        }

        if (SeePlayer(hit) && !shotCooldown)
        {
            StartCoroutine(shootAfterSeconds());
        }

        UpdateAnimationState();
    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TurnBox")
        {
            tempWalkSpeed = -tempWalkSpeed;
            facingRight = !facingRight;
            walkSpeed = -walkSpeed;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void Shoot()
    {
        if (!shot)
        {
            Instantiate(bulletPrefab, projOffset.position, projOffset.rotation);
        }
    }

    bool SeePlayer(bool hit)
    {
        bool vision = false;
        if (hit)
        {
            vision = true;
        }
        return vision;
    }

    IEnumerator shootAfterSeconds()
    {
        walkSpeed = 0f;
        shotCooldown = true;
        shot = false;
        yield return new WaitForSeconds(5);
        Shoot();
        shot = true;
        StartCoroutine(cooldown());
        walkSpeed = tempWalkSpeed;
    }

    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(3);
        shotCooldown = false;
    }

    void UpdateAnimationState()
    {
        MovementState state;
        if (walkSpeed != 0f)
        {
            state = MovementState.walking;
        }
        else
        {
            state = MovementState.charging;
        }

        anim.SetInteger("state", (int)state);
    }
}
