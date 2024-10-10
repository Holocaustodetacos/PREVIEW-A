using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 35f; // Velocidad del jugador
    public float jumpForce = 10f; // Fuerza del salto
    public Transform groundCheck; // Detector de suelo
    public float groundCheckRadius = 0.2f; // Radio de verificación de suelo
    public LayerMask groundLayer; // Capa para detectar suelo
    public Rigidbody2D rb; // Rigidbody para el personaje
    private Animator animator; // Controlador de animaciones
    private Vector2 movement; // Movimiento del personaje
    private bool isGrounded; // Para verificar si el personaje está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del personaje
        animator = GetComponent<Animator>(); // Obtiene el Animator del personaje
    }

    void Update()
    {
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
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Movimiento del jugador en FixedUpdate usando Rigidbody2D
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
    }

    private void Flip()
    {
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;
    }
}