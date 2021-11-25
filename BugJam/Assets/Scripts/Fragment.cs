using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : bullet
{
    public CircleCollider2D collider = null;
    private float rotateSpeed;
    void Start()
    {
        collider = gameObject.GetComponent<CircleCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        lifeLeft = life;
        sr.color = new Color(0f, 0f, 0f, 1f);
        rotateSpeed = Random.Range(-5f, 5f);
    }

    public override void SpecificAction()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed));
        if (dead)
        {
            lifeLeft -= 3;
        }
    }

    public override void wallBounce(Collision2D other)
    {
        dead = true;
        collider.enabled = false;
    }
}
