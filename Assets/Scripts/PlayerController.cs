using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 25f; //Velocidad del jugador
    private Rigidbody2D rb; //Hitbox
    private Animator animator; //Animacion

    void Start() {
        rb = GetComponent<Rigidbody2D>(); //Obtiene controlador rigidbody
        animator = GetComponent<Animator>(); //Obtiene controlador animator
    }

    void Update() {
        //Captura info de player
        float moverHorizontal = Input.GetAxis("Horizontal");

        //Crear vector de movimiento
        Vector2 movement = new Vector2(moverHorizontal, 0.0f);

        //Mover al jugador
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);

        //Controlar animacion Walking
        animator.SetBool("isWalking", movement.magnitude > 0);
        animator.SetBool("facingLeft", moverHorizontal < 0);
    }
}
