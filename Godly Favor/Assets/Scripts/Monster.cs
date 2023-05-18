using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 13f;
    public float interactionDistance = 2f;
    public float maxHealth = 50f;
    public float currentHealth = 50f;
    public float damage = 10f;

    public int animationSpeed = 3;
    public bool isGrounded;
    public bool isMoving;
    public float distanceToPlayer;
    public float detectionDistance = 10f;
    public float attackDistance = 1.2f;
    public int attackDelay = 1;
    public bool isAttacking = false;
    public bool isTargetingPlayer = false;

    public Vector2 destinationDelta;
    public Vector2 destination;
    public bool hasReachedDestination = true;

    public Sprite idleSprite;
    public Sprite[] walkSprites = new Sprite[2];
    public PlayerController player;
    public Game game;

    // [HideInInspector]
    public Rigidbody2D rb;
    // [HideInInspector] 
    public SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        
        if (!isTargetingPlayer)
        {
            if (hasReachedDestination)
            {
                destinationDelta = new Vector2(Random.Range(-7f, 7f), Random.Range(0f, 0f));
                destination = new Vector2(transform.position.x + destinationDelta.x, transform.position.y + destinationDelta.y);
                hasReachedDestination = false;
                isMoving = true;
            }
            else if (Vector2.Distance(transform.position, destination) < 0.5f || Vector2.Distance(transform.position, destination) > 14f)
            {
                isMoving = false;
                destination = transform.position;
                StartCoroutine(IdleDelay());
            }
            else 
            {
                // This is where moving happens
                // TODO: Add jumping
                transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            }

            if (distanceToPlayer <= detectionDistance)
            {
                isTargetingPlayer = true;
            }
        }
        else
        {
            destination = player.transform.position;
            if (distanceToPlayer > detectionDistance)
            {
                isTargetingPlayer = false;
                hasReachedDestination = true;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            }
        }

        ChangeSprite();
        Attack();
    }

    IEnumerator IdleDelay()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        hasReachedDestination = true;
    }

    public void ChangeSprite()
    {
        if (destination.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        if (isMoving)
        {
            sr.sprite = walkSprites[(int)(Time.time * animationSpeed) % 2];
        }
        else
        {
            sr.sprite = idleSprite;
        }
    }

    public void Attack()
    {
        if (distanceToPlayer <= attackDistance && !isAttacking)
        {
            isAttacking = true;
            player.TakeDamage(damage);
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // TODO: Check if works
            Destroy(this);
        }
    }

}
