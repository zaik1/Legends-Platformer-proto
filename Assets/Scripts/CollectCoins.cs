using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoins : MonoBehaviour
{
    [Tooltip("The particles that appear after the player collects a coin.")]
    public GameObject coinParticles;

    private PlayerMovement playerMovementScript;
    private int coinsCollected = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerMovementScript = other.GetComponent<PlayerMovement>();
            playerMovementScript.soundManager.PlayCoinSound();
            ScoreManager.score += 10;
            coinsCollected++;

            // Spawn coin particles
            GameObject particles = Instantiate(coinParticles, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f); // Destroy the particles after 1 second

            Destroy(gameObject);

            if (coinsCollected >= 10)
            {
                RestoreHealth(other.gameObject); // Pass the player GameObject to RestoreHealth
                coinsCollected = 0; // Reset the counter after restoring health
            }
        }
    }

    // Method to restore player's health
    void RestoreHealth(GameObject player)
    {
        // Find the PlayerHealth component and add 10 health
        GetHit playerHealth = player.GetComponent<GetHit>();
        if (playerHealth != null)
        {
            playerHealth.Heal(10);
        }
    }
}