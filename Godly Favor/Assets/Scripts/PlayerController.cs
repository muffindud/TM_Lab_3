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

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChangeSprite();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        // float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxis("Horizontal");
        float veritcal = Input.GetAxisRaw("Vertical");
        float jump = Input.GetAxisRaw("Jump");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
        {
            sr.flipX = false;
        }
        else if (horizontal < 0)
        {
            sr.flipX = true;
        }

        if ((veritcal > 0.1f || jump > 0.1f) && isGrounded == true)
        {
            movement.y = jumpForce;
        }

        rb.velocity = movement;
        
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

    public void Movement()
    {

    }
    
}
