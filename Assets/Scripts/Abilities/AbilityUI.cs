using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro

public class AbilityUI : MonoBehaviour
{
    [Header("References")]
    public Image cooldownOverlay; // Asigna el Image en modo Fill
    public TMP_Text cooldownText; // O Text si no usas TMPro
    
    public void UpdateCooldown(float currentCooldown, float maxCooldown)
    {
        if(maxCooldown <= 0) return;
        
        // Calcular el porcentaje de cooldown
        float fillAmount = currentCooldown / maxCooldown;
        cooldownOverlay.fillAmount = fillAmount;
        
        // Actualizar texto (opcional)
        if(currentCooldown > 0)
        {
            cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
        }
        else
        {
            cooldownText.text = "";
        }
    }
}