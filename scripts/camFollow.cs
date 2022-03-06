using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 camOffset;
    public Rigidbody rb;

    void Update()
    {
        rb.MovePosition(new Vector3(target.position.x + camOffset.x, transform.position.y, target.position.z + camOffset.z));
    }
}
