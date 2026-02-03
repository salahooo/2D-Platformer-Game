using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;

    [Header("Jump Settings")]
    public float jumpForce = 14f;
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 3f;
    public float gravityScale = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Advanced Jumping")]
    public float coyoteTime = 0.12f;       // Extra jump window after walking off ledge
    public float jumpBufferTime = 0.12f;   // Allows pressing jump slightly before landing

    [Header("Lives System")]
    public int lives = 3;
    public Transform respawnPoint;
    public TextMeshProUGUI livesText;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    // Internal components
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float coyoteCounter = 0f;       // Timer tracking how long after leaving ground you can still jump
    private float jumpBufferCounter = 0f;   // Timer storing early jump presses
    private bool isGrounded = false;

    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();

        UpdateLivesUI();

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

    // Handles left/right movement input
    public void OnMove(InputValue value)
    {
        if (isDead) return;

        moveInput = value.Get<Vector2>();
        HandleSpriteFlip();
    }

    // Stores jump input for jump buffering
    public void OnJump(InputValue value)
    {
        if (isDead) return;

        if (value.isPressed)
            jumpBufferCounter = jumpBufferTime; 
    }

    private void Update()
    {
        if (isDead) return;

        HandleGroundCheck();
        ApplyCoyoteTime();      
        HandleJumpBuffering();  
        ApplyBetterJumpPhysics();
        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        MovePlayer();
    }

    // Flips sprite depending on movement direction
    private void HandleSpriteFlip()
    {
        if (moveInput.x > 0.1f) spriteRenderer.flipX = false;
        else if (moveInput.x < -0.1f) spriteRenderer.flipX = true;
    }

    // Updates animation parameters each frame
    private void UpdateAnimatorParameters()
    {
        anim.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("verticalVelocity", rb.linearVelocity.y);
    }

    // Moves the player horizontally
    private void MovePlayer()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    // Checks if the character is grounded
    private void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position, 
            groundCheckRadius, 
            groundLayer
        );
    }

    // Counts down coyote-time window
    private void ApplyCoyoteTime()
    {
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;
    }

    // Executes buffered jump if coyote-time allows it
    private void HandleJumpBuffering()
    {
        jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            Jump();
            jumpBufferCounter = 0f;
        }
    }

    // Applies upward jump force
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteCounter = 0f; 
    }

    // Faster falling + variable jump height
    private void ApplyBetterJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Reduces lives when taking damage
    public void TakeDamage()
    {
        if (isDead) return;

        lives--;
        UpdateLivesUI();

        if (lives > 0) Respawn();
        else GameOver();
    }

    // Updates "Lives: X" UI text
    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    // Teleports player to respawn point
    private void Respawn()
    {
        transform.position = respawnPoint.position;
    }

    // Handles Game Over logic
    private void GameOver()
    {
        isDead = true;
        rb.simulated = false;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Shows win screen
    private void Win()
    {
        isDead = true;
        rb.simulated = false;
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Restart button logic
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
