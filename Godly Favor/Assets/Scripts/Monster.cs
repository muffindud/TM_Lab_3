using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 13f;
    public float interactionDistance = 2f;
    public float maxHealth = 200f;
    public float currentHealth = 200f;
    public float damage = 10f;

    public int animationSpeed = 3;
    public bool isGrounded;
    public bool isMoving;
    public float distanceToPlayer;

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
    }
}
