using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Vector3 velocity;
    public float life = 200;
    public float fadeStart = 50;
    public SpriteRenderer sr = null;

    public bool weird = false;
    protected float lifeLeft;
    private bool xFlip;
    private bool yFlip;
    private bool frame1;
    protected bool piercing = false;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        lifeLeft = life;
        frame1 = true;
        if (weird)
        {
            float temp = velocity.y;
            velocity.y = velocity.x;
            velocity.x = temp;
        }
        frame1 = false;
        float b = 0;
        if (weird)
        { b = 1; }
        sr.color = new Color(0f, 0f, b, 1f);

    }
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        xFlip = false;
        yFlip = false;
        lifeLeft -= 1;
        if (weird)
        {
            transform.position += new Vector3(velocity.y, velocity.x, 0f);
        }
        else
        {
            transform.position += velocity;
        }


        if (lifeLeft < fadeStart)
        {
            float b = 0;
            if (weird)
            { b = 1; }
            sr.color = new Color(0f, 0f, b, (lifeLeft / fadeStart) + 0.1f);
            if (lifeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void hit()
    {
        if (!piercing)
        {
            Destroy(gameObject);
        }
    }

    // called when the bullet hits a wall
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {

            if (frame1)
            {
                Destroy(gameObject);
            }
            float x = other.GetContact(0).normal.x;
            float y = other.GetContact(0).normal.y;
            if (Mathf.Abs(x) > Mathf.Abs(y) + 0.05)
            {
                if (xFlip == false)
                {
                    velocity.x = -velocity.x;
                    xFlip = true;
                }
            }
            else if (Mathf.Abs(x) < Mathf.Abs(y) - 0.05)
            {
                if (yFlip == false)
                {
                    velocity.y = -velocity.y;
                    yFlip = true;
                }
            }
            else
            {
                if (yFlip == false)
                {
                    velocity.y = -velocity.y;
                    yFlip = true;
                }
                if (xFlip == false)
                {
                    velocity.x = -velocity.x;
                    xFlip = true;
                }
            }
        }
    }
}