using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // Time delay before the platform starts falling after the player touches it
    public float fallWait = 2f;

    // Time delay before the fallen platform is destroyed
    public float destroyWait = 1f;

    private Rigidbody2D rb;
    private bool isFalling; // Ensures the platform falls only once

    void Start()
    {
        // Cache the Rigidbody2D so we can change the body type when it should fall
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player touched the platform AND the platform hasn't started falling yet
        if (!isFalling && collision.collider.CompareTag("Player"))
            StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        isFalling = true;

        // Wait some time before letting the platform fall (delay before breaking)
        yield return new WaitForSeconds(fallWait);

        // Change to Dynamic so gravity affects it and it drops
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Destroy the platform after it has fallen
        Destroy(gameObject, destroyWait);
    }
}
