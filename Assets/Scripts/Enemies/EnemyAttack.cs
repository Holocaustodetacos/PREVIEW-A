using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    private float lastAttackTime;
    private Transform playerTransform;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private bool applyKnockback = true;
    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        if(Time.time > lastAttackTime + attackCooldown)
        {
            if(PlayerInRange())
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }
    
    bool PlayerInRange()
    {
        if(playerTransform == null) return false;
        
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= attackRange;
    }
    
    void Attack()
    {
        // Verificar si el jugador está en rango nuevamente
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        
        foreach(Collider2D player in hitPlayers)
        {
            HealthSystem playerHealth = player.GetComponent<HealthSystem>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemigo atacó al jugador por " + attackDamage + " de daño");
            }
        }
        
        // Aquí puedes añadir animación de ataque
    }
    
    // Dibujar el rango de ataque en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if(playerHealth != null)
            {
                // Pasa la posición del enemigo como origen del knockback
                playerHealth.TakeDamage(damageAmount, transform.position);
            }
        }
    }
}