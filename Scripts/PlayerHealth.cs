using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 10;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Example condition to trigger game over
        if (playerHealth <= 0)
        {
            gameManager.GameOver();
        }

        // For testing: Press 'G' to trigger game over
        if (Input.GetKeyDown(KeyCode.G))
        {
            gameManager.GameOver();
        }
    }

    // Method to reduce health, called by enemies when they reach the end
    public void TakePlayerDamage(int damage)
    {
        playerHealth -= damage;
    }
}