using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public  float rotationSpeed;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate() {
        rb.angularVelocity = Vector3.up * 1;
    }
}
