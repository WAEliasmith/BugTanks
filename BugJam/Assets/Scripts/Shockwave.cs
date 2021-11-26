using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : Explosion
{
    public float explodeStart = 75;

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (angularRotationSpeed > 0)
        {
            RotateToVelocity();
        }
        else
        {
            float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), bullet.velocity);
            transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
        }

        float squeeze = Mathf.Min(stretchMax, 1f + Mathf.Pow(Mathf.Abs(bullet.velocity.magnitude), 2f) * stretchFromVelocityMod);
        float stretch = 1f / squeeze;


        life -= 1;
        sr.color = color;
        if (life <= fadeStart)
        {
            color.a = (life / fadeStart);
            scale = Mathf.Max((life / fadeStart) * size * 1.3f, 0.1f);
        }

        if (life >= fadeEnd)
        {
            color.a = ((maxLife - fadeEnd) - (life - fadeEnd)) / (maxLife - fadeEnd);
            scale = Mathf.Max((((maxLife - fadeEnd) - (life - fadeEnd)) / (maxLife - fadeEnd)) * size * 1.3f, 0.1f);
        }
        else
        {
            if (life % 8 == 0)
            {
                spreadBlocksInExplosion((int)Mathf.Round(size + 1), (int)Mathf.Round(size + 1f), "TYW", true);
                spreadBlocksInExplosion((int)Mathf.Round(size + 1), (int)Mathf.Round(size + 1f), "TXW", true);

            }
        }
        transform.localScale = new Vector3(scale * stretch, scale * squeeze, 1f);
        if (life <= 0)
        {
            Destroy(gameObject);
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
    public float stretchFromVelocityMod = 0.2f;
    public float stretchMax = 2f;
    private Rigidbody2D rb;
    public float angularRotationSpeed = 1;

    public bullet bullet;

    void RotateToVelocity()
    {
        float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), bullet.velocity);
        float angle = transform.eulerAngles.z;
        float dangle = Mathf.DeltaAngle(angle, targetAngle);
        transform.eulerAngles = new Vector3(0f, 0f, angle + dangle);
    }
}
