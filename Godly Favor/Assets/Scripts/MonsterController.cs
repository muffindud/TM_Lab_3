using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 10f;
    public int animationSpeed = 3;
    public float interactionDistance = 2f;
    
    public bool isGrounded;
    public bool isMoving;

    public Sprite idleSprite;
    public Sprite[] walkSprites = new Sprite[2];

    public Game game;
    public PlayerController player;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D cc;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        ChangeSprite();
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
        // TODO
    }

    public void ChangeSprite()
    {
        if (isMoving)
        {
            sr.sprite = walkSprites[(int)(Time.time * animationSpeed) % 2];
        }
        else
        {
            sr.sprite = idleSprite;
        }
    }
}
