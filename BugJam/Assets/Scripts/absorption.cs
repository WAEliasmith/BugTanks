using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class absorption : bullet
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            wallBounce(other);
        }
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.GetComponent<bullet>().hit();
            hit();
        }
    }
}
