using UnityEngine;

public class Arrow : MonoBehaviour
{ private float destroyDelay = 5f;   // Now visible in inspector
    
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyDelay);     // Much cleaner than Invoke
    }

    // Changed this entire fixedupdate to make my arrow fly straight
    void FixedUpdate()
    {
        if (_rb.linearVelocity.magnitude > 0.1f)
        {
            transform.forward = _rb.linearVelocity.normalized;
            transform.Rotate(-90, 0, 0);
        }
    }
}