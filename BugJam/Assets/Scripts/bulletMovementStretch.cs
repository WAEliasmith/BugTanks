using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMovementStretch : MonoBehaviour
{
    public float stretchFromVelocityMod = 0.2f;
    public float stretchMax = 2f;
    private Rigidbody2D rb;
    public float angularRotationSpeed = 1;

    public bullet bullet;

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
        float stretch = Mathf.Min(stretchMax, 1f + Mathf.Pow(Mathf.Abs(bullet.velocity.magnitude), 2f) * stretchFromVelocityMod);
        float squeeze = 1f / stretch;
        transform.localScale = new Vector3(stretch, squeeze, 1f);
    }

    void RotateToVelocity()
    {
        float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), bullet.velocity);
        float angle = transform.eulerAngles.z;
        float dangle = Mathf.DeltaAngle(angle, targetAngle);
        transform.eulerAngles = new Vector3(0f, 0f, angle + dangle);
    }
}
