using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCarrier : bullet
{
    public float minVelocity = 0;
    public float maxVelocity = 1.6f;
    public float acceleration = 1.02f;

    // Start is called before the first frame update
    void Start()
    {
        piercing = true;
        if (speedOverride > 0)
        {
            velocity = velocity.normalized * speedOverride;
        }
        lifeLeft = life;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {

        if (velocity.magnitude < maxVelocity && velocity.magnitude > minVelocity)
        {
            velocity *= acceleration;
        }
        lifeLeft -= 1;
        if (lifeLeft <= 0)
        {
            Destroy(gameObject);
        }
        transform.position += velocity;

    }
}
