using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Transform particleSpawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Handle"))
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(particlePrefab, particleSpawnPos.position, Quaternion.identity, FindObjectOfType<Container>().transform);
                GasManager.instance.IncreaseParticleCount(5);
            }
        }
    }
}
