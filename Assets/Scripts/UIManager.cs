using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI livesText;   // Assign in Inspector
    public GameObject gameOverPanel;    // Assign in Inspector
    public GameObject winPanel;         // Assign in Inspector

    private void Start()
    {
        UpdateLives(3);    // Initial value
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    // Called by PlayerMovement when lives change
    public void UpdateLives(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }
}
