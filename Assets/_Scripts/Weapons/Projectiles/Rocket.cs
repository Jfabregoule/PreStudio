using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Rigidbody rb;
    public float initialSpeed = 5f;
    public float accelerationRate = 2f;
    public float maxSpeed = 20f;
    public float lifetime = 5f;
    private float currentSpeed;

    void Start()
    {
        rb.velocity = transform.forward * initialSpeed;

        currentSpeed = initialSpeed;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationRate * Time.fixedDeltaTime;
        }
        rb.velocity = transform.forward * currentSpeed;
    }
}
