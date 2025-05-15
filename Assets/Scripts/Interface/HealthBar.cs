using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthSystem))]
public class HealthBar : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    
    [Header("Colores por Nivel")]
    [SerializeField] private Color highHealthColor = Color.green;    // Por encima del 80%
    [SerializeField] private Color mediumHealthColor = new Color(1f, 0.65f, 0f); // Naranja
    [SerializeField] private Color lowHealthColor = Color.red;       // Por debajo del 30%
    
    [Header("Umbrales")]
    [SerializeField] [Range(0, 1)] private float mediumHealthThreshold = 0.8f;
    [SerializeField] [Range(0, 1)] private float lowHealthThreshold = 0.3f;
    
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
        healthSlider.value = healthSystem.currentHealth;
        UpdateHealthColor();
    }

    private void UpdateHealthColor()
    {
        if(fillImage == null) return;
        
        float normalizedHealth = healthSlider.normalizedValue;
        
        if(normalizedHealth > mediumHealthThreshold)
        {
            // Transición entre verde y naranja
            float t = (normalizedHealth - mediumHealthThreshold) / (1f - mediumHealthThreshold);
            fillImage.color = Color.Lerp(mediumHealthColor, highHealthColor, t);
        }
        else
        {
            // Transición entre naranja y rojo
            float t = normalizedHealth / lowHealthThreshold;
            fillImage.color = Color.Lerp(lowHealthColor, mediumHealthColor, t);
        }
    }

    private void OnDestroy()
    {
        if(healthSystem != null)
        {
            healthSystem.onDamageTaken.RemoveListener(UpdateHealthUI);
            healthSystem.onHeal.RemoveListener(UpdateHealthUI);
        }
    }
}