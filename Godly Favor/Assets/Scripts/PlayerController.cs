using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool isGrounded;
    public bool isMoving;
    
    public Sprite idleSprite;
    public Sprite movingSprite_1;
    public Sprite movingSprite_2;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    // Public functions
    public void Movement()
    {

    }
    
}
