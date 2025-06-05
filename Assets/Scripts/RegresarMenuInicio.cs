using UnityEngine;
using UnityEngine.SceneManagement;

public class RegresarMenuInicio : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuInicio"); // Nombre de tu escena del menú
    }
}
