using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionControl : MonoBehaviour
{
    public Rigidbody rb;
    public float radius = 5.0F;
    public float power = 10.0F;
    Vector3 explosionPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        explosionPos = transform.position;
    }

    void OnCollisionEnter()
    {
        //rb.AddRelativeForce(Vector3.forward, ForceMode.Impulse);
        rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
    }
}
