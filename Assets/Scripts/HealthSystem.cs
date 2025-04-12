using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Damage")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool isPlayer = false;
    public UnityEvent onDeath;
    public UnityEvent<int> onDamageTaken;
    public UnityEvent<int> onHeal;

    [Header("Knockback")]
    public bool applyKnockback = true;
    public Knockback knockbackSystem;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        onDamageTaken.Invoke(damage);

        // Aplicar knockback si está configurado
        if(applyKnockback && knockbackSystem != null)
        {
            knockbackSystem.ApplyKnockback(damageSourcePosition);
        }
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    // Mantén tu TakeDamage original para compatibilidad
    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHeal.Invoke(amount);
    }

    private void Die()
    {
        onDeath.Invoke();
        
        if(isPlayer)
        {
            // Lógica cuando el jugador muere
            Debug.Log("Jugador derrotado");
            // Puedes reiniciar el nivel o mostrar game over
        }
        else
        {
            // Lógica cuando el enemigo muere
            Debug.Log("Enemigo derrotado");
            Destroy(gameObject);
        }
    }
}