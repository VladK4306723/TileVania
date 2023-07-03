using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myEnemyRigidbody;
    BoxCollider2D myEnemyBoxCollider;
    void Start()
    {
        myEnemyRigidbody = GetComponent<Rigidbody2D>();
        myEnemyBoxCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        myEnemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
       
        transform.localScale = new Vector3(-(Mathf.Sign(myEnemyRigidbody.velocity.x)), 1f);
        
    }
}
