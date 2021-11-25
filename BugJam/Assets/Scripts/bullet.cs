using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float ownerScoreNumber;
    public Vector3 velocity;
    public float life = 200;
    public float fadeStart = 50;
    public SpriteRenderer sr = null;

    public bool weird = false;
    protected float lifeLeft;
    protected bool xFlip;
    protected bool yFlip;
    public float strength = 1f;
    public bool piercing = false;
    public bool dead = false;
    public float speedOverride = 0;

    public Color color = new Color(0f, 0f, 0f, 1f);

    void Start()
    {
        if (speedOverride > 0)
        {
            velocity = velocity.normalized * speedOverride;
        }
        sr = gameObject.GetComponent<SpriteRenderer>();
        lifeLeft = life;
        if (weird)
        {
            float temp = velocity.y;
            velocity.y = velocity.x;
            velocity.x = temp;
        }
        float b = 0;
        if (weird)
        { color.b = 1; }
    }
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        lifeLeft -= 1;
        if (lifeLeft < fadeStart)
        {
            color.a = (lifeLeft / fadeStart) + 0.1f;
            if (lifeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
        sr.color = color;
        if (!dead)
        {
            SpecificAction();
            xFlip = false;
            yFlip = false;
            if (weird)
            {
                transform.position += new Vector3(velocity.y, velocity.x, 0f);
            }
            else
            {
                transform.position += velocity;
            }
        }
    }

    public virtual void SpecificAction()
    {
        //here so it can be overrided
    }

    public virtual void hit()
    {
        if (!piercing)
        {
            Destroy(gameObject);
        }
    }

    public virtual void wallBounce(Collision2D other)
    {
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

    // called when the bullet hits a wall
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            wallBounce(other);
        }
    }
}