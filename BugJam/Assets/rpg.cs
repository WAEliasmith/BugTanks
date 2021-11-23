using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpg : bullet
{
    public GameObject explosion = null;
    public float maxVelocity = 4;
    public float acceleration = 1.02f;
    public float startSpeed = 0.8f;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        velocity = velocity.normalized * startSpeed;
        lifeLeft = life;
        sr.color = new Color(1f, 0f, 0f, 1f);
    }
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        lifeLeft -= 1;
        transform.position += velocity;
        if (velocity.magnitude < maxVelocity)
        {
            velocity *= acceleration;
        }


        if (lifeLeft < fadeStart)
        {
            sr.color = new Color(1f, 0f, 0f, (lifeLeft / fadeStart) + 0.1f);
            if (lifeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    public override void hit()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // called when the rpg hits a wall
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            hit();
        }
    }
}