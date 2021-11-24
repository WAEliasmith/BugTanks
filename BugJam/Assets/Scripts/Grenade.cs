using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : rpg
{
    // called when the grenade hits a wall
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            wallBounce(other);
        }
    }
}


