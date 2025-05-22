using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Configuración")]
    public int maxHealth = 100;
    [SerializeField] private int _currentHealth; // Ahora privado con serialización
    public bool isPlayer = false;
    
    [Header("Eventos")]
    public UnityEvent onDeath;
    public UnityEvent<int> onDamageTaken; // Envía la vida ACTUAL después del daño
    public UnityEvent<int> onHeal; // Envía la vida ACTUAL después de la cura

    [Header("Invencibilidad")]
    public float invincibilityDuration = 0.5f;
    private bool isInvincible = false;
    private float invincibilityEndTime;

    // Propiedad para acceso controlado
    public int currentHealth {
        get => _currentHealth;
        private set {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    void Start()
    {
        _currentHealth = maxHealth; // Inicialización directa
        Debug.Log($"Vida inicializada: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        if(isInvincible && Time.time < invincibilityEndTime) 
        {
            Debug.Log("Daño bloqueado por invencibilidad");
            return;
        }
        
        int previousHealth = currentHealth;
        currentHealth -= damage;
        
        isInvincible = true;
        invincibilityEndTime = Time.time + invincibilityDuration;
        
        // Ahora envía la vida ACTUAL, no el daño recibido
        onDamageTaken.Invoke(currentHealth);
        Debug.Log($"Daño recibido: {damage}. Vida actual: {currentHealth}/{maxHealth}");
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        int previousHealth = currentHealth;
        currentHealth += amount;
        
        if(currentHealth > previousHealth)
        {
            onHeal.Invoke(currentHealth);
        }
    }

    public void RestoreFullHealth()
    {
        Heal(maxHealth);
    }

    private void Die()
    {
        onDeath.Invoke();
        Debug.Log(isPlayer ? "Jugador derrotado" : "Enemigo derrotado");
        
        if(!isPlayer) Destroy(gameObject);
    }
}