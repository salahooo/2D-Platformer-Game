using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public int lives = 3;
    public Transform respawnPoint;

    [Header("UI")]
    public TMP_Text livesText;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    public static System.Action OnPlayerRespawn;

    bool isDead = false;

    // Set this to the LAST build index
    int lastLevelIndex = 2; // Level03 is index 2

    private void Start()
    {
        UpdateLivesUI();
    }

    public void TakeDamage()
    {
        if (isDead) return;

        lives--;
        UpdateLivesUI();

        if (lives > 0) Respawn();
        else GameOver();
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        OnPlayerRespawn?.Invoke();
    }

    void GameOver()
    {
        isDead = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdateLivesUI()
    {
        livesText.text = "Lives: " + lives;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("WIN"))
            HandleWinCondition();

        if (col.CompareTag("Enemy"))
            TakeDamage();

        if (col.CompareTag("Death"))
            TakeDamage();
    }

    void HandleWinCondition()
    {
        int current = SceneManager.GetActiveScene().buildIndex;

        if (current < lastLevelIndex)
        {
            // Go to next level
            SceneManager.LoadScene(current + 1);
        }
        else
        {
            // Final level completed
            Win();
        }
    }

    void Win()
    {
        isDead = true;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("YOU WIN!");
    }
}
