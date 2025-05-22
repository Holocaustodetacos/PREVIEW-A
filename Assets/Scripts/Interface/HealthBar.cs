using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(HealthSystem))]
public class HealthBar : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Configuración")]
    [Range(0f, 1f)] public float highThreshold = 0.8f;
    [Range(0f, 1f)] public float lowThreshold = 0.3f;
    [SerializeField] private Color highHealthColor = Color.green;
    [SerializeField] private Color mediumHealthColor = new Color(1f, 0.65f, 0f);
    [SerializeField] private Color lowHealthColor = Color.red;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        
        // Configuración inicial segura
        if(healthSlider != null)
        {
            healthSlider.minValue = 0;
            healthSlider.maxValue = healthSystem.maxHealth;
            healthSlider.value = healthSystem.maxHealth;
        }
    }

    private void Start()
    {
        // Suscripción a eventos con inicialización explícita
        healthSystem.onDamageTaken.AddListener(UpdateHealthUI);
        healthSystem.onHeal.AddListener(UpdateHealthUI);
        
        // Fuerza actualización inicial
        UpdateHealthUI(healthSystem.currentHealth);
    }

    private void UpdateHealthUI(int currentHealth)
    {
        // Actualización segura de todos los componentes
        if(healthSlider != null) 
        {
            healthSlider.value = currentHealth;
        }
        
        UpdateHealthColor(currentHealth);
        
        if(healthText != null)
        {
            healthText.text = $"{currentHealth}/{healthSystem.maxHealth}";
        }
        
        Debug.Log($"UI Actualizada - Vida: {currentHealth}/{healthSystem.maxHealth}");
    }

    private void UpdateHealthColor(int currentHealth)
    {
        if(fillImage == null) return;
        
        float healthPercentage = (float)currentHealth / healthSystem.maxHealth;
        
        if(healthPercentage > highThreshold)
        {
            fillImage.color = highHealthColor;
        }
        else if(healthPercentage > lowThreshold)
        {
            fillImage.color = mediumHealthColor;
        }
        else
        {
            fillImage.color = lowHealthColor;
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