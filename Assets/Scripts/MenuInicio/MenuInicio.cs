using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    // Nombre de la escena del juego (asegúrate de que coincida con el nombre en Build Settings)
    public string nombreEscenaJuego = "SampleScene";

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego); // Carga la escena del juego
    }

    public void Salir()
    {
        Application.Quit();
        
        // (Opcional) Para debuggear en el Editor:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}