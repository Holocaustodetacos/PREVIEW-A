using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [Header("Configuración")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public float stunDuration = 0.1f; // Tiempo adicional sin control
    
    [Header("Efectos")]
    public ParticleSystem knockbackParticles;
    public bool freezeXAxis = false;
    public bool freezeYAxis = false;
    
    // Variables privadas
    private bool isKnockedBack = false;
    private float knockbackEndTime;
    private Vector2 knockbackDirection;
    private Rigidbody2D rb;
    //private PlayerMovement playerMovement; // Opcional: referencia a movimiento
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //playerMovement = GetComponent<PlayerMovement>();
    }
    
    public void ApplyKnockback(Vector2 sourcePosition)
    {
        if(isKnockedBack) return;
        
        // Calcular dirección
        knockbackDirection = (Vector2)transform.position - sourcePosition;
        knockbackDirection = knockbackDirection.normalized;
        
        // Aplicar restricciones de ejes
        if(freezeXAxis) knockbackDirection.x = 0;
        if(freezeYAxis) knockbackDirection.y = 0;
        
        // Aplicar fuerza
        if(rb != null)
        {
            rb.velocity = knockbackDirection * knockbackForce;
        }
        
        // Desactivar control del jugador (opcional)
       /*  if(playerMovement != null) */
       /*  { */
       /*      playerMovement.SetMovementEnabled(false); */
       /*  } */
       /*   */
        // Efectos visuales
        if(knockbackParticles != null)
        {
            knockbackParticles.Play();
        }
        
        isKnockedBack = true;
        knockbackEndTime = Time.time + knockbackDuration;
    }
    
    void Update()
    {
        if(isKnockedBack && Time.time >= knockbackEndTime)
        {
            EndKnockback();
        }
    }
    
    private void EndKnockback()
    {
        if(rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        
        // Reactivar control después de un breve stun
        /* if(playerMovement != null) */
        /* { */
        /*     StartCoroutine(EnableMovementAfterDelay()); */
        /* } */
        isKnockedBack = false;
    }
    
    /* private IEnumerator EnableMovementAfterDelay() */
    /* { */
    /*     yield return new WaitForSeconds(stunDuration); */
    /*     playerMovement.SetMovementEnabled(true); */
    /* } */
}