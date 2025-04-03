using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private float knockbackEndTime;
    private Vector2 knockbackDirection;
    private Rigidbody2D rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void ApplyKnockback(Vector2 sourcePosition)
    {
        if(isKnockedBack) return;
        
        knockbackDirection = (Vector2)transform.position - sourcePosition;
        knockbackDirection = knockbackDirection.normalized;
        
        if(rb != null)
        {
            rb.velocity = knockbackDirection * knockbackForce;
        }
        
        isKnockedBack = true;
        knockbackEndTime = Time.time + knockbackDuration;
    }
    
    void Update()
    {
        if(isKnockedBack && Time.time >= knockbackEndTime)
        {
            if(rb != null)
            {
                rb.velocity = Vector2.zero;
            }
            isKnockedBack = false;
        }
    }
}