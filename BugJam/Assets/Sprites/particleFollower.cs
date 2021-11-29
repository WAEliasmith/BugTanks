using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleFollower : MonoBehaviour
{
    public float life = 100;
    public Transform target;
    private ParticleSystem ps;

    void Start()
    {
        transform.position = target.position;
        transform.eulerAngles = target.eulerAngles;
        ps = GetComponent<ParticleSystem>();
    }
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (target)
        {
            transform.position = target.position;
            transform.eulerAngles = target.eulerAngles;
        }
        else
        {
            ps.Stop();
            life = 0;
        }
        life--;
        if (life == 0)
        {
            ps.Stop();
        }
        if (life <= -500)
        {
            Destroy(gameObject);
        }
    }
}
