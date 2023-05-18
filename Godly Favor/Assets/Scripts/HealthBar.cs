using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public PlayerController player;

    void Update()
    {
       transform.localScale = new Vector2(5 * player.currentHealth / player.maxHealth, 0.5f); 
    }
}
