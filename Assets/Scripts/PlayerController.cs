using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D),typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuración Básica")]
    public int vidas = 3;
    public Transform puntoRespawn;
    public LayerMask groundLayer;
    public bool canMove = true;

    [Header("Movimiento")]
    public float speed = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private Vector2 movement;
    private bool isGrounded;

    [Header("Salto")]
    public float jumpForce = 6f;
    public float maxJumpForce = 10f;
    private float currentJumpForce = 0f;
    private bool isJumping = false;
    private bool canDoubleJump = false;
    private bool hasJumped = false;

    [Header("Caída")]
    public float fastFallSpeed = 10f;
    private bool isFalling = false;
    private bool isFallingFast = false;

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public ParticleSystem dashParticles;
    private bool isDashing = false;
    private float dashEndTime;
    private float nextDashTime = 0f;
    private Vector2 dashDirection;

    [Header("Animaciones")]
    private Animator animator;
    private bool isIdle = false;

    [Header("Referencias")]
    private Rigidbody2D rb;

    [Header("Eventos")]
    public UnityEvent onLivesChanged;

    #region Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!canMove) return;

        HandleDash();
        HandleMovement();
        HandleJump();
        HandleFastFall();
        HandleAnimations();
        HandleAttack();
    }

    void FixedUpdate()
    {
        if(isDashing)
        {
            rb.velocity = dashDirection * (dashDistance / dashDuration);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canDoubleJump = true;
            ResetJumpState();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
    #endregion

    #region Movement Methods
    private void HandleMovement()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float moveInput = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(moveInput, rb.velocity.y);
    }

    private void HandleDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime && !isDashing)
        {
            StartDash();
        }

        if(isDashing && Time.time >= dashEndTime)
        {
            EndDash();
        }
    }

    private void StartDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Dashing");
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;
        
        float moveInput = Input.GetAxisRaw("Horizontal");
        dashDirection = moveInput != 0 ? 
            new Vector2(moveInput, 0) : 
            new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);
        
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        
        if(dashParticles != null) dashParticles.Play();
        animator.SetTrigger("Dash");
    }

    private void EndDash()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        isDashing = false;
        rb.gravityScale = 1;
        rb.velocity = dashDirection * (speed * 0.5f);
    }
    #endregion

    #region Jump Methods
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !hasJumped)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            currentJumpForce += jumpForce * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0f, maxJumpForce);
        }

        if (Input.GetKeyUp(KeyCode.W) && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            currentJumpForce = 0f;
            isJumping = false;
        }
    }

    private void Jump()
    {
        if (isGrounded && !hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);         
            hasJumped = true;
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
    }

    private void ResetJumpState()
    {
        isJumping = false;
        currentJumpForce = 0f;
        hasJumped = false;
        animator.SetBool("isJumping", false);
    }
    #endregion

    #region Other Mechanics
    private void HandleFastFall()
    {
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
            isFallingFast = true;
            animator.SetBool("isFallingFast", true);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Attack", true);
        }
    }

    private void HandleAnimations()
    {
        animator.SetBool("isWalking", movement.magnitude > 0);
        animator.SetBool("facingLeft", movement.x < 0);
        animator.SetBool("facingRight", movement.x > 0);

        if (isGrounded)
        {
            isFalling = false;
            isFallingFast = false;
            animator.SetBool("isFalling", false);
            animator.SetBool("isFallingFast", false);
        }
        else if (rb.velocity.y < 0 && !isFallingFast)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }

        animator.SetBool("isIdle", movement.x == 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S));
    }

    public void Morir()
    {
        transform.position = puntoRespawn.position;
        GetComponent<SpriteRenderer>().enabled = true;
        rb.velocity = Vector2.zero;
        
        if (vidas > 0)
        {
            vidas--;
            GetComponent<HealthSystem>()?.RestoreFullHealth();
            onLivesChanged.Invoke();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if(!enabled) rb.velocity = Vector2.zero;
    }
    #endregion
}