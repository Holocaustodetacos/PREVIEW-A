using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetX = 2f;
    public float smoothSpeed = 0.125f; 
    void Start()
    {
        
    }

    void Update() {
        Vector3 desiredPosition = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothPosition;
    }
}
