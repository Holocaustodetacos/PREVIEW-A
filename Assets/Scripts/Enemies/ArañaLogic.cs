using UnityEngine;

public class ArañaLogic : MonoBehaviour
{
    public float life = 100;
    public float walkSpeed = 2f; // Velocidad de caminata
    public float jumpForce = 2f; // Fuerza del salto
    public float detectionRange = 5f; // Rango de detección del jugador
    public LayerMask playerLayer; // Capa del jugador
    public float cooldown = 2f; // Tiempo de espera entre saltos
    public Transform[] patrolPoints; // Puntos de patrulla (izquierda y derecha)
    public float waitTime = 1f; // Tiempo de espera en cada punto de patrulla

    private Rigidbody2D rb;
    private Transform player;
    private bool isGrounded;
    private float lastJumpTime;
    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;
    private bool isWaiting = false;
    private float waitTimer = 0f;

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
            isPatrolling = false; // Deja de patrullar
            FlipSprite(); // Voltea el sprite hacia el jugador

            // Verifica si la araña está en el suelo y si ha pasado el tiempo de cooldown
            if (isGrounded && Time.time - lastJumpTime >= cooldown)
            {
                Jump();
            }
        }
        else
        {
            isPatrolling = true; // Vuelve a patrullar
            Patrol();
        }
    }

    void Patrol()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Cambia al siguiente punto de patrulla
            }
        }
        else
        {
            // Mueve la araña hacia el punto de patrulla actual
            Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

            // Voltea el sprite según la dirección del movimiento
            if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1); // Derecha
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // Izquierda
            }

            // Si llega al punto de patrulla, espera un momento
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWaiting = true;
            }
        }
    }

    void FlipSprite()
    {
        // Compara la posición del jugador con la posición de la araña
        if (player.position.x > transform.position.x)
        {
            // Jugador a la derecha: voltear el sprite hacia la derecha
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            // Jugador a la izquierda: voltear el sprite hacia la izquierda
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Jump()
    {
        // Calcula la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Aplica la fuerza de salto
        rb.AddForce(new Vector2(direction.x * jumpForce, jumpForce), ForceMode2D.Impulse);

        // Actualiza el tiempo del último salto
        lastJumpTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si la araña está en el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica si la araña ha dejado el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}