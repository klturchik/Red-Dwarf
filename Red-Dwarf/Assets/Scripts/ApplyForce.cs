using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour {

    public float forceX = 0;
    public float forceY = 0;
    public float forceZ = 0;
    public float rotateX = 0;
    public float rotateY = 0;
    public float rotateZ = 0;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(transform.right * forceX);
        rb.AddForce(transform.up * forceY);
        rb.AddForce(transform.forward * forceZ);

        rb.transform.Rotate(new Vector3(rotateX, rotateY, rotateZ));
    }
}
