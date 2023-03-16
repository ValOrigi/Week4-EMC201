using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float projSpeed;
    public Rigidbody2D rigidBody2D;

    // Update is called once per frame
    void Update()
    {
        rigidBody2D.velocity = transform.right * projSpeed;
        Destroy(gameObject, .5f);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.name == "Character")
        {
            Destroy(gameObject);
        }
    }
}
