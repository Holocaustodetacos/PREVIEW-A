using UnityEngine;

public class Boss_1_Logic : MonoBehaviour
{
    public float dashSpeed = 15f;       // Velocidad del dash
    public float dashDuration = 0.3f;   // Duración del movimiento
    public float dashCooldown = 2f;     // Tiempo de espera entre dashes
    public bool dashTowardsPlayer = true; // ¿Dash hacia el jugador?
    public float detectionRange = 5f; // Rango de detección del jugador

    private Rigidbody2D rb;
    private Transform player;
    private bool canDash = true;
    private bool isDashing = false;
    public LayerMask playerLayer; // Capa del jugador

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que el jugador tenga el tag "Player"
    }

    void Update()
    {
        // Verifica si el jugador está en rango
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
               StartDash();
            }
        }   

    // Llama a este método desde un Animation Event o cuando necesites dash
    public void StartDash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;

            // Calcula dirección del dash
            Vector2 dashDirection = dashTowardsPlayer ? 
                (player.position - transform.position).normalized : 
                transform.right;

            rb.velocity = dashDirection * dashSpeed;

            Invoke(nameof(StopDash), dashDuration);
        }
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero;
        isDashing = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    // Opcional: Dibuja el rango de dash en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 2f);
    }
}