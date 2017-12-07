using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private Quaternion drehung;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
}