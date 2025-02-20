using UnityEngine;

public class PlayerC_2 : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 6f;
    public float fastFallSpeed = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isFallingFast = false;
    private bool isIdle = false;
    private int auxJump = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar si el jugador está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimiento horizontal
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(moveHorizontal, rb.velocity.y);
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

        // Actualizar animaciones de movimiento
        animator.SetBool("isWalking", Mathf.Abs(moveHorizontal) > 0);
        animator.SetBool("facingLeft", moveHorizontal < 0);
        animator.SetBool("facingRight", moveHorizontal > 0);

        // Lógica de salto
        if (isGrounded)
        {
            auxJump = 0;  // Reiniciar contador de saltos cuando el jugador toca el suelo
            isJumping = false;
            isFalling = false;
            isFallingFast = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isFallingFast", false);
        }
        else if (rb.velocity.y < 0 && !isFallingFast)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }

        // Saltar si el jugador está en el suelo o puede hacer un doble salto
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded || auxJump < 2)
            {
                Jump();
                auxJump++;
            }
        }

        // Caída rápida
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
            isFallingFast = true;
            animator.SetBool("isFallingFast", true);
        }

        // Actualizar animación de inactividad
        if (Mathf.Abs(moveHorizontal) == 0 && isGrounded && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            isIdle = true;
            animator.SetBool("isIdle", true);
        }
        else
        {
            isIdle = false;
            animator.SetBool("isIdle", false);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        animator.SetBool("isJumping", true);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el área de detección de suelo en el editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
