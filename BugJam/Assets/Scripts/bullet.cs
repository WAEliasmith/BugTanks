using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public bool veryDangerous = false;
    public GameObject destroyedsplosion = null;

    public AudioSource bonkSound;

    public int ownerScoreNumber;
    public Vector3 velocity;
    public float life = 200;
    public float fadeStart = 50;
    public SpriteRenderer sr = null;

    public bool weird = false;
    public float lifeLeft;
    protected bool xFlip;
    protected bool yFlip;
    public float strength = 1f;
    public bool piercing = false;
    public bool dead = false;
    public float speedOverride = 0;

    public int maxBounces = -1;
    public int wallCounter = 0;
    public int noweirdnextframe = 0;

    public Color color = new Color(0f, 0f, 0f, 1f);

    void Start()
    {
        if (speedOverride > 0)
        {
            velocity = velocity.normalized * speedOverride;
        }
        if (sr == null)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
        }
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
        noweirdnextframe--;
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
        if (maxBounces != -1 && wallCounter >= maxBounces)
        {
            hit();
        }
    }

    public virtual void hit()
    {
        if (!piercing)
        {
            if (destroyedsplosion)
            {
                GameObject p = Instantiate(destroyedsplosion, transform.position, Quaternion.identity);
                p.transform.position = transform.position;
                float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), velocity);
                p.transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
            }
            Destroy(gameObject);
        }
    }

    public void bonk()
    {
        if (bonkSound && (wallCounter < maxBounces || maxBounces == -1))
        {
            bonkSound.pitch = Random.Range(0.8f, 1f);
            bonkSound.Play();
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
                wallCounter += 1;
                bonk();
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
                wallCounter += 1;
                bonk();
            }
        }
        else
        {
            if (xFlip == false || yFlip == false)
            {
                wallCounter += 1;
                bonk();
            }
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

    void LateUpdate()
    {

    }

    // called when the bullet hits a wall
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            if (noweirdnextframe <= 0)
            {
                wallBounce(other);
            }
        }
    }
}