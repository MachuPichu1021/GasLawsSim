using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasParticle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Random.insideUnitCircle.normalized;
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        if (GasManager.instance.Temperature != 0)
            rb.velocity = 0.01f * GasManager.instance.Temperature * rb.velocity.normalized;
    }

    public void Freeze()
    {
        direction = rb.velocity.normalized;
        speed = rb.velocity.magnitude;
        rb.velocity = Vector2.zero;
    }

    public void Unfreeze()
    {
        rb.velocity = direction * speed;
    }
}
