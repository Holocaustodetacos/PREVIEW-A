using UnityEngine;

public class VacioMortal : MonoBehaviour
{
    public Transform puntoRespawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entró es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtiene el componente Jugador (si existe)
            PlayerController jugador = other.GetComponent<PlayerController>();
            
            if (jugador != null)
            {
                jugador.Morir(); // Llama a la función Morir del jugador
            }
            else
            {
                // Si no hay script Jugador, simplemente lo teletransporta
                other.transform.position = puntoRespawn.position;
            }
        }
    }
}