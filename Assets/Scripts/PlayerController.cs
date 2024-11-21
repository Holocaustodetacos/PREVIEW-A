using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad del jugador
    public float jumpForce = 5f; // Fuerza de salto inicial
    public float maxJumpForce = 10f; // Fuerza máxima de salto (máxima altura que puede alcanzar)
    public float jumpHoldTime = 0.2f; // Tiempo máximo de acumulación de salto
    public Transform groundCheck; // Detector de suelo
    public float groundCheckRadius = 0.2f; // Radio de verificación de suelo
    public LayerMask groundLayer; // Capa para detectar suelo
    public Rigidbody2D rb; // Rigidbody para el personaje
    private Animator animator; // Controlador de animaciones
    private Vector2 movement; // Movimiento del personaje
    private bool isGrounded; // Para verificar si el personaje está en el suelo

    private float currentJumpForce = 0f; // Fuerza de salto actual (acumulada)
    private bool isJumping = false; // Estado de si el personaje está saltando
    private bool canDoubleJump = false; // Si el jugador puede hacer doble salto

    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del personaje
        animator = GetComponent<Animator>(); // Obtiene el Animator del personaje
    }

    void Update()
    {
        Debug.Log("isJumping: " + isJumping);
        Debug.Log("canDoubleJump: " + canDoubleJump);
        Debug.Log("isFalling: " + isFalling);

        // Verificar si el personaje está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Captura la información de movimiento del jugador
        float moverHorizontal = Input.GetAxisRaw("Horizontal");

        // Crear vector de movimiento
        movement = new Vector2(moverHorizontal, 0.0f);

        // Mover al jugador
        rb.velocity = new Vector2(movement.x * speed * Time.deltaTime, rb.velocity.y);

        // Controlar la animación Walking
        animator.SetBool("isWalking", movement.magnitude > 0);
        animator.SetBool("facingLeft", moverHorizontal < 0);
        animator.SetBool("facingRight", moverHorizontal > 0);

        // Controlar el salto
        if (isGrounded)
        {
            // Resetear las condiciones cuando el personaje está en el suelo
            canDoubleJump = true; // El jugador puede hacer doble salto
            currentJumpForce = 0f; // Resetear la fuerza acumulada del salto
            isJumping = false; // Marcar que ya no está saltando cuando toca el suelo
            isFalling = false; // El jugador no está cayendo
            animator.SetBool("isFalling", false); // Desactivar animación de caída
        }
        else
        {
            // Si no está en el suelo, verificar si está cayendo
            if (rb.velocity.y < 0) // El jugador está cayendo (velocidad en Y negativa)
            {
                isFalling = true;
                animator.SetBool("isFalling", true); // Activar animación de caída
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) // Si está subiendo pero la tecla de salto ya no está presionada
            {
                isFalling = false;
                animator.SetBool("isFalling", false); // Desactivar animación de caída si está subiendo
            }
        }

        // Detectar si presionas la tecla de salto
        if (Input.GetKey(KeyCode.Space))
        {
            // Solo acumula fuerza si está en el suelo o si puede hacer un doble salto
            if (isGrounded || (canDoubleJump && !isGrounded))
            {
                // Acumular la fuerza de salto mientras la tecla está presionada
                currentJumpForce += jumpForce * Time.deltaTime;

                // Limitar la fuerza máxima de salto
                currentJumpForce = Mathf.Clamp(currentJumpForce, 0f, maxJumpForce);
            }
        }

        // Saltar cuando se presiona la tecla (y está en el suelo o puede hacer doble salto)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                isJumping = true; // Marcar que el jugador está saltando
                animator.SetBool("isJumping", true); // Activar animación de salto
            }
            else if (canDoubleJump)
            {
                // Si no está en el suelo pero tiene el power-up de doble salto
                DoubleJump();
                canDoubleJump = false; // Deshabilitar el doble salto después de usarlo
                isJumping = true; // Asegurar que se mantenga el estado de salto
                animator.SetBool("isJumping", true); // Activar animación de salto
            }
        }

        // Si el jugador suelta la tecla, aplicar la fuerza acumulada
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Aplicar la fuerza acumulada
            if (isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                currentJumpForce = 0f; // Resetear la fuerza acumulada
                isJumping = false; // Marcar que ya no está saltando
                animator.SetBool("isJumping", false); // Desactivar animación de salto
            }
        }
    }

    void FixedUpdate()
    {
        // Movimiento del jugador en FixedUpdate usando Rigidbody2D
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        // Aplicar la fuerza inicial de salto
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void DoubleJump()
    {
        // Aplicar la fuerza de salto extra (doble salto)
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f); // Puedes ajustar la fuerza aquí
    }

    // Método para detectar la colisión con el suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el jugador colisiona con un objeto etiquetado como "Ground"
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true; // Marcar que está tocando el suelo
            canDoubleJump = true; // Permitir el doble salto si vuelve a tocar el suelo
            isJumping = false; // Terminar el estado de salto cuando toca el suelo
            animator.SetBool("isJumping", false); // Desactivar animación de salto
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Si el jugador deja de tocar el suelo, actualizar el estado de isGrounded
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false; // Marcar que ya no está en el suelo
        }
    }
}
