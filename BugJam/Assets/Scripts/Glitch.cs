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
    public float glitchTimer;

    private float glitching = 0f;

    void FixedUpdate()
    {
        glitching--;
        glitchTimer--;
        transform.position = owner.transform.position;
        transform.eulerAngles = owner.transform.eulerAngles;
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
    public void OnCollisionStay2D(Collision2D other)
    {
        if (glitching == -3)
        {
            if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
            {

                glitchTimer = 18f;
                Particle();
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (glitchTimer <= 0)
        {
            if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
            {
                glitching = 1f;
            }
        }
    }

}
