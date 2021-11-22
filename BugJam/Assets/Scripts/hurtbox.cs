using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public float maxHp = 3;

    public float hp;

    public Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public void IHaveBeenHit()
    {
        //tell the hurtbox they have been hit
        hp -= 1;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (other.GetComponent<bullet>().dead = true)
            {
                IHaveBeenHit();
                other.GetComponent<bullet>().hit();
            }
        }

        if (other.tag == "Powerup")
        {
            if (gun.powerup == "none")
            {
                gun.powerup = other.GetComponent<Powerup>().powerup;
                other.GetComponent<Powerup>().Collected();
            }
        }
    }
}
