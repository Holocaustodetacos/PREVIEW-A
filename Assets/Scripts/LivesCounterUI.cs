using UnityEngine;
using TMPro;

public class LivesCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        UpdateLivesText();
    }

    public void UpdateLivesText()
    {
        if(livesText != null && playerController != null)
        {
            livesText.text = $"{playerController.vidas}";
        }
    }
}