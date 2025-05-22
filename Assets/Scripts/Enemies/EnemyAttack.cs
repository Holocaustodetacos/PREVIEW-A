using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public int attackDamage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    private float lastAttackTime;
    private bool hasDamagedInThisAttack = false;
    
    [Header("Knockback")]
    public bool applyKnockback = true;
    public float knockbackForce = 5f;
    
    private Transform playerTransform;
    
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
        return playerTransform != null && 
               Vector2.Distance(transform.position, playerTransform.position) <= attackRange;
    }
    
    void Attack()
    {
        hasDamagedInThisAttack = false; // Resetear el flag
        
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        
        foreach(Collider2D player in hitPlayers)
        {
            if(!hasDamagedInThisAttack) // Solo dañar una vez por ataque
            {
                HealthSystem playerHealth = player.GetComponent<HealthSystem>();
                if(playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                    hasDamagedInThisAttack = true;
                    Debug.Log("Ataque conectado - Daño aplicado");
                }
            }
        }
    }
    
    private void ApplyKnockback(Rigidbody2D playerRb, Vector2 direction)
    {
        if(playerRb != null)
        {
            playerRb.velocity = Vector2.zero; // Resetear velocidad primero
            playerRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}