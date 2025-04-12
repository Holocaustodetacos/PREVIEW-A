// HealthBar.cs
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthSystem))]
public class HealthBar : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Colores")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;
    
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        // Configurar valores iniciales
        healthSlider.maxValue = healthSystem.maxHealth;
        healthSlider.value = healthSystem.currentHealth;
        UpdateHealthColor();
        
        // Suscribirse a eventos
        healthSystem.onDamageTaken.AddListener(UpdateHealthUI);
        healthSystem.onHeal.AddListener(UpdateHealthUI);
    }

    private void UpdateHealthUI(int _)
    {
        // El parámetro de daño/curación no lo usamos aquí
        healthSlider.value = healthSystem.currentHealth;
        UpdateHealthColor();
    }

    private void UpdateHealthColor()
    {
        if(fillImage != null)
        {
            fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, 
                                       healthSlider.normalizedValue);
        }
    }

    private void OnDestroy()
    {
        // Importante: desuscribirse de eventos para evitar memory leaks
        if(healthSystem != null)
        {
            healthSystem.onDamageTaken.RemoveListener(UpdateHealthUI);
            healthSystem.onHeal.RemoveListener(UpdateHealthUI);
        }
    }
}