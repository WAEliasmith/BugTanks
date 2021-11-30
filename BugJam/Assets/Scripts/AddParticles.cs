using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddParticles : MonoBehaviour
{
    public GameObject particles = null;
    public float life = 100;

    // Start is called before the first frame update
    void Start()
    {
        GameObject p = Instantiate(particles, transform.position, Quaternion.identity);
        p.transform.position = transform.position;
        p.GetComponent<particleFollower>().target = transform;
        p.GetComponent<particleFollower>().life = life;
    }
}
