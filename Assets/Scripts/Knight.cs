using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public float moveSpeed = 4f;                           
    public float maxSpeed = 4f;                            
    public float moveStopSpeed = 0.6f;                     
    public float knockbackForce = 10f;                     
    
    Rigidbody2D rb;                                         
    TouchingDirections touchingDirections;                  
    Animator animator;                                     
    Damageable damageable;                                 
    
    public DetectionZone attackZone;                       
    public DetectionZone cliffDetectionZone;               
    public enum WalkableDirection { Right, Left }           
    private WalkableDirection _walkDirection;               
    private Vector2 walkDirectionVector = Vector2.right;    
    public bool _HasTarget = false;                        

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool HasTarget
    {
        get { return _HasTarget; }
        private set
        {
            _HasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }
    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (HasTarget)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (touchingDirections.isGrounded && touchingDirections.isOnWall)
            {
                FlipDirection();
            }

            if (!damageable.LockVelocity)
            {
                if (CanMove)
                {
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (moveSpeed * walkDirectionVector.x * Time.deltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, moveStopSpeed), rb.velocity.y);
                }
            }
        }
    }

    private void FlipDirection()
    {
        if (HasTarget) return;

        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.Log("Current walkable direction is not set to legal values of right and left");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = Vector2.zero;
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
        {
            FlipDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        Damageable player = collision.collider.GetComponent<Damageable>();
        if (player != null)
        {
            Debug.Log("Archer hit Player via collision!");

            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;

            player.Hit(10, knockbackDir);

            Collider2D archerCol = GetComponent<Collider2D>();
            Collider2D playerCol = collision.collider;

            Physics2D.IgnoreCollision(archerCol, playerCol, true);
            StartCoroutine(ReenableCollision(archerCol, playerCol, 2f));
        }
    }

    private IEnumerator ReenableCollision(Collider2D a, Collider2D b, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(a, b, false);
    }


}
