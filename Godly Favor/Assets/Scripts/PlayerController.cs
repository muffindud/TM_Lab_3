using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float jumpForce = 13f;
    public int animationSpeed = 3;

    public bool isGrounded;
    public bool isMoving;

    public Sprite idleSprite;
    public Sprite[] walkSprites = new Sprite[2];

    public Vector2Int mousePos;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    

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
        // float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Horizontal");
        float veritcal = Input.GetAxisRaw("Vertical");
        float jump = Input.GetAxisRaw("Jump");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if ((veritcal > 0.1f || jump > 0.1f) && isGrounded == true)
        {
            movement.y = jumpForce;
        }

        rb.velocity = movement;
        
        sr.flipX = horizontal < 0;
        isMoving = movement.x != 0;
    }

    // Public functions
    public void ChangeSprite()
    {
        if (isMoving)
        {
            sr.sprite = walkSprites[0 + (int)(Time.time * animationSpeed) % 2];
        }
        else
        {
            sr.sprite = idleSprite;
        }
    }

}
