using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class IceWave : MonoBehaviour
{
    public int damage = 20;
    private Rigidbody2D rb;
    private TouchingDirections touching;

    private bool isLanded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touching = GetComponent<TouchingDirections>();
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (!isLanded && touching.isGrounded)
        {
            Land();
        }
    }

    private void Land()
    {
        isLanded = true;
        rb.velocity = Vector2.zero;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();

        if (collision.CompareTag("Player")) { 
            dmg.Hit(damage, Vector2.zero);


                PlayerFreezeHandler freeze = collision.GetComponent<PlayerFreezeHandler>();
                if (freeze != null)
                {
                    freeze.Freeze();
                }
            
        }
    }

}
