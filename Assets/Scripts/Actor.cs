using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int maxHealth;

    public int currentHealth { get; private set; }

    void Awake()
    { currentHealth = maxHealth; }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Took {amount}. Current health: {currentHealth}");
        if (currentHealth <= 0)
        { Death(); }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
