using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 6f;
    public float maxJumpForce = 10f;
    public float fastFallSpeed = 20f; // Nueva variable para la velocidad de caída rápida
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isGrounded;
    private float currentJumpForce = 0f;
    private bool isJumping = false;
    private bool canDoubleJump = false;
    private bool isFalling = false;
    private bool isFallingFast = false; // Nueva variable para caída rápida
    private bool isIdle = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moverHorizontal = Input.GetAxisRaw("Horizontal");

        // Crear vector de movimiento
        movement = new Vector2(moverHorizontal, rb.velocity.y);
        rb.velocity = new Vector2(movement.x * speed * Time.deltaTime, rb.velocity.y);

        animator.SetBool("isWalking", movement.magnitude > 0);
        animator.SetBool("facingLeft", moverHorizontal < 0);
        animator.SetBool("facingRight", moverHorizontal > 0);

        if (isGrounded)
        {
            canDoubleJump = true;
            currentJumpForce = 0f;
            isJumping = false;
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
        else if (rb.velocity.y > 0)
        {
            isFalling = false;
            animator.SetBool("isFalling", false);
        }

        // Saltar solo si el jugador está en el suelo o puede hacer doble salto
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || (canDoubleJump && !isGrounded)))
        {
            Jump();
            isJumping = true;
            if (!isGrounded && canDoubleJump)
            {
                canDoubleJump = false;
            }
            animator.SetBool("isJumping", true);
        }

        // Ajustar la fuerza del salto mientras se mantiene presionada la tecla de salto
        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            currentJumpForce += jumpForce * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0f, maxJumpForce);
        }

        // Aplicar la fuerza del salto cuando se suelta la tecla de salto
        if (Input.GetKeyUp(KeyCode.W) && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            currentJumpForce = 0f;
            isJumping = false;
        }

        // Detectar si se presiona la tecla "S" para descender rápidamente
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed); // Ajusta la velocidad de descenso rápido
            isFallingFast = true;
            animator.SetBool("isFallingFast", true); // Activar animación de caída rápida
        }

        // Actualizar animación de caída según la velocidad vertical
        if (rb.velocity.y < 0 && !isFalling && !isFallingFast)
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }
        else if (rb.velocity.y >= 0 && (isFalling || isFallingFast))
        {
            isFalling = false;
            isFallingFast = false;
            animator.SetBool("isFalling", false);
            animator.SetBool("isFallingFast", false);
        }

        // Detectar si el jugador está inactivo
        if (moverHorizontal == 0 && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
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

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canDoubleJump = true;
            isJumping = false;
            isFalling = false;
            isFallingFast = false;
            isIdle = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isFallingFast", false);
            animator.SetBool("isIdle", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
