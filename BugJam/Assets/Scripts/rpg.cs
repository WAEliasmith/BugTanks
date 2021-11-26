using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rpg : bullet
{
    public GameObject explosion = null;
    public float maxVelocity = 4;
    public float minVelocity = 0f;
    public float acceleration = 1.02f;
    public float startSpeed = 0.8f;
    public float size;
    public float explodeStart = 10f;

    void Start()
    {
        velocity = velocity.normalized * startSpeed;
        lifeLeft = life;
        color = new Color(1f, 0f, 0f, 1f);
    }

    public override void SpecificAction()
    {
        if (velocity.magnitude < maxVelocity && velocity.magnitude > minVelocity)
        {
            velocity *= acceleration;
        }
        if (lifeLeft <= 1)
        {
            hit();
        }
        if (lifeLeft < explodeStart)
        {
            float scale = Mathf.Max((lifeLeft / explodeStart) * size, 0.01f);
            transform.localScale = new Vector3(scale, scale, 1f);
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