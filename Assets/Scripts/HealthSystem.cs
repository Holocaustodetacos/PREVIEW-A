using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isPlayer = false;
    
    public UnityEvent onDeath;
    public UnityEvent<int> onDamageTaken;
    public UnityEvent<int> onHeal;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        onDamageTaken.Invoke(damage);
        
        if(currentHealth <= 0)
        {
            Die();
        }
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