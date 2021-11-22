using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Vector3 velocity;
    public float life = 200;
    public float fadeStart = 50;
    public SpriteRenderer sr;

    public bool weird = false;
    public bool piercing = false;
    private float lifeLeft;
    private GridLayout gridLayout;
    private GridLayout gridLayoutY;
    private bool xFlip;
    private bool yFlip;
    private bool frame1;

    public bool dead = false;
    public bool hitted = false;
    public bool visible = true;

    void Start()
    {
        lifeLeft = life;
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        gridLayoutY = GameObject.Find("YWalls").GetComponent<GridLayout>();
        frame1 = true;
        if (weird)
        {
            float temp = velocity.y;
            velocity.y = velocity.x;
            velocity.x = temp;
        }

    }
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (!dead)
        {
            if (lifeLeft < life)
            {
                frame1 = false;
                float b = 0;
                if (weird)
                { b = 1; }
                sr.color = new Color(0f, 0f, b, 1f);
                if (!visible)
                { sr.color = new Color(0f, 0f, 0f, 0f); }
            }
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
                if (!visible)
                { sr.color = new Color(0f, 0f, 0f, 0f); }
                if (lifeLeft <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

    }

    public void hit()
    {
        if (!piercing)
        {
            Destroy(gameObject);
        }
        hitted = true;
    }

    // called when the player hits an enemyWeapon
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