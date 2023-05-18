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
    public int newDestinationDelay = 5;
    public bool isAttacking = false;
    public bool isTargetingPlayer = false;

    public Vector2 destinationDelta;
    public Vector2 destination;
    public bool hasReachedDestination = true;

    public Sprite idleSprite;
    public Sprite[] walkSprites = new Sprite[2];
    public PlayerController player;
    public Game game;
    public MonsterController monsterController;

    // [HideInInspector]
    public Rigidbody2D rb;
    // [HideInInspector] 
    public SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        game = GameObject.Find("GameWorld").GetComponent<Game>();
        monsterController = GameObject.Find("MonsterController").GetComponent<MonsterController>();
        idleSprite = monsterController.monsterSpriteStatic;
        walkSprites[0] = monsterController.monsterSpriteWalk1;
        walkSprites[1] = monsterController.monsterSpriteWalk2;
        this.gameObject.name = "Monster";
        this.gameObject.tag = "Monster";
        StartCoroutine(DestinationDelay());
        StartCoroutine(DespawnDelay());
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
                GenerateDestinationDelta();
                destination = new Vector2(transform.position.x + destinationDelta.x, transform.position.y + destinationDelta.y);
                hasReachedDestination = false;
                isMoving = true;
            }
            else if (Vector2.Distance(transform.position, destination) < 0.5f || Vector2.Distance(transform.position, destination) > 30f)
            {
                isMoving = false;
                destination = transform.position;
                StartCoroutine(IdleDelay());
            }
            else 
            {
                // This is where moving happens
                
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

    public void GenerateDestinationDelta()
    {
        // Radomly generate a destination delta vector to valid destination
        float deltaX = Random.Range(7f, 20f) * (Random.Range(0, 2) * 2 - 1);
        float deltaY = 0f;
        
        destinationDelta = new Vector2(deltaX, deltaY);
    }

    IEnumerator DestinationDelay()
    {
        yield return new WaitForSeconds(newDestinationDelay);
        hasReachedDestination = true;
        StartCoroutine(DestinationDelay());
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
        Knockback();
        game.IncreaseBellumScore();
        if (currentHealth <= 0)
        {
            // TODO: Check if works
            game.IncreaseBellumScore();
            monsterController.monsterCount--;
            Destroy(this.gameObject);
        }
    }

    IEnumerator DespawnDelay()
    {
        yield return new WaitForSeconds(60);
        monsterController.monsterCount--;
        Destroy(this.gameObject);
    }

    public void Knockback()
    {
        Vector2 knockbackDirection = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
        rb.AddForce(knockbackDirection * 500f);
    }
}
