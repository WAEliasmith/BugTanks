using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    public GameObject owner;
    public GameObject particles = null;
    public AudioSource sound = null;
    public float soundTop = 1f;
    public float soundBottom = 0.8f;
    public bool protag = false;

    void FixedUpdate()
    {
        transform.position = owner.transform.position;
    }

    // Start is called before the first frame update
    void Particle()
    {
        if (protag)
        {
            MenuManager.instance.currChrome = 0.6f;
        }
        if (particles)
        {
            GameObject p = Instantiate(particles, transform.position, Quaternion.identity);
            p.transform.position = transform.position;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            Particle();
        }
    }
}
