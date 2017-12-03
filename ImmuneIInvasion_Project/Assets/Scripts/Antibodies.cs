using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antibodies : MonoBehaviour
{
    ParticleSystem ps;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Bacteria>() != null)
        {
            collision.GetComponent<Bacteria>().Disable();
        }
    }
}
