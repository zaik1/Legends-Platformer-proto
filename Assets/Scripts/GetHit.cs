using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHit : MonoBehaviour
{
    [Tooltip("Determines when the player is taking damage.")]
    public bool hurt = false;

    public int maxHealth = 100; // Maximum health of the player
    public int currentHealth; // Current health of the player

    private bool slipping = false;
    private PlayerMovement playerMovementScript;
    private Rigidbody rb;
    private Transform enemy;

    
    
    // Method to heal the player by a specified amount
    public void Heal(int amount)
    {
        currentHealth += amount;
        // Ensure current health does not exceed max health
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        currentHealth = maxHealth; // Initialize current health to max health
    }

    private void FixedUpdate()
    {
        // stops the player from running up the slopes and skipping platforms
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
            playerMovementScript.playerStats.canMove = false;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (hurt == false)
        {
            if (other.gameObject.tag == "Enemy")
            {
                enemy = other.gameObject.transform;
                rb.AddForce(enemy.forward * 1000);
                rb.AddForce(transform.up * 500);
                TakeDamage(10); // Example: Player takes 10 damage when colliding with an enemy
            }
            if (other.gameObject.tag == "Trap")
            {
                rb.AddForce(transform.forward * -1000);
                rb.AddForce(transform.up * 500);
                TakeDamage(20); // Example: Player takes 20 damage when colliding with a trap
            }
        }
        if (other.gameObject.layer == 9)
        {
            slipping = true;
        }
        if (other.gameObject.layer != 9)
        {
            if (slipping == true)
            {
                slipping = false;
                playerMovementScript.playerStats.canMove = true;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Decrease current health by damage amount

        if (currentHealth <= 0)
        {
            Die(); // Call Die method if health drops to or below 0
        }
        else
        {
            hurt = true;
            playerMovementScript.playerStats.canMove = false;
            playerMovementScript.soundManager.PlayHitSound();
            StartCoroutine("Recover");
        }
    }

    private void Die()
    {
        // Perform any death-related actions here, such as showing game over screen, respawning, etc.
        Debug.Log("Player died!");
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.75f);
        hurt = false;
        playerMovementScript.playerStats.canMove = true;
    }
}