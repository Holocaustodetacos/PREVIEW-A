using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;
    public float maxJumpForce = 10f;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log("isGrounded: " + isGrounded);
        Debug.Log("isJumping: " + isJumping);
        Debug.Log("canDoubleJump: " + canDoubleJump);
        Debug.Log("isFalling: " + isFalling);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("GroundCheck position: " + groundCheck.position);
        Debug.Log("GroundCheck radius: " + groundCheckRadius);
        Debug.Log("GroundLayer: " + groundLayer);

        float moverHorizontal = Input.GetAxisRaw("Horizontal");
        float moverVertical = Input.GetAxisRaw("Vertical");

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
            animator.SetBool("isFalling", false);
        }
        else
        {
            if (rb.velocity.y < 0)
            {
                isFalling = true;
                animator.SetBool("isFalling", true);
            }
            else if (rb.velocity.y > 0)
            {
                isFalling = false;
                animator.SetBool("isFalling", false);
            }
        }

        if (moverVertical > 0)
        {
            if (isGrounded || (canDoubleJump && !isGrounded))
            {
                currentJumpForce += jumpForce * Time.deltaTime;
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0f, maxJumpForce);
            }
        }

        if (moverVertical > 0 && isGrounded)
        {
            Jump();
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
        else if (moverVertical > 0 && canDoubleJump)
        {
            DoubleJump();
            canDoubleJump = false;
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

        if (moverVertical == 0 && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
            currentJumpForce = 0f;
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        // Detectar si se presiona la tecla "S" para descender rápidamente
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed * 2); // Ajusta la velocidad de descenso según sea necesario
            animator.SetBool("isFalling", true); // Activar animación de caída
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

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            canDoubleJump = true;
            isJumping = false;
            animator.SetBool("isJumping", false);
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
