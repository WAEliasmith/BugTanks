using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    public Vector2 velocity;
    public Transform[] body;
    public int time = 0;

    public float angle;

    public hurtbox hbox;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hbox.hp <= 0)
        {
            gameObject.SetActive(false);
        }
        //move head
        transform.position += (Vector3)velocity;
        //edit velocity
        velocity = velocity * 0.8f;
        time++;
        if (Mathf.Round(time * 0.01f) % 2 == 0)
        {
            angle++;
        }
        else
        {
            angle -= 0.8f;
        }
        float speed = velocity.magnitude;

        speed += 0.006f;

        float dangle = Mathf.DeltaAngle(Vector2.SignedAngle(new Vector2(1f, 0f), velocity), angle);
        angle += dangle * 0.1f;
        velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle),
                    Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * speed;
    }
}
