using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteBullets : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.GetComponent<bullet>().hit();
        }
    }
}
