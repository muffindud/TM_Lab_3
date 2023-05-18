using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRange : MonoBehaviour
{
    public PlayerController player;
    public int attackDelay = 1; // seconds
    public bool isAttacking = false;

    public float maxHealth = 50f;
    public float currentHealth = 50f;
    public float damage = 10f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            player.currentHealth -= damage / player.defenseMultiplier;
            // set isAttacking to false after attackDelay seconds
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
}
