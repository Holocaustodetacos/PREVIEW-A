using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // Referencia al jugador
    public float offsetX = 100f;        // Desplazamiento en el eje X
    public float offsetY = 2f;        // Desplazamiento en el eje Y (puedes ajustarlo según lo que quieras)
    public float smoothSpeed = 0.5f; // Velocidad de suavizado del movimiento de la cámara

    void Start() {}

    void Update() 
    {
        // Calculamos la posición deseada, ahora considerando tanto el eje X como el Y
        Vector3 desiredPosition = new Vector3(player.position.x + offsetX, player.position.y + offsetY, transform.position.z);

        // Suavizamos el movimiento para que se mueva de manera más fluida
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualizamos la posición de la cámara
        transform.position = smoothPosition;
    }
}
