using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float arrowSpeed = 20f;
    GameObject player;
    float xSpeed;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
