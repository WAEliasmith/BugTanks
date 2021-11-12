using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public float maxHp = 3;

    public float hp;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            hp -= 1;
            other.GetComponent<bullet>().hit();
        }
    }
}
