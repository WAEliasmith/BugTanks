using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1 : MonoBehaviour
{
    public hurtbox hbox = null;
    public GameObject shockwave = null;
    public GameObject gunPos;

    public bool goTime = true;

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    // Update is called once per frame
    void Update()
    {
        if (hbox && hbox.hp <= 3 && goTime)
        {
            goTime = false;
            for (int i = 0; i < 9; i++)
            {
                GameObject p = Instantiate(shockwave, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<bullet>().velocity = Rotate((Vector2)gunPos.transform.right, (i - 4) * 20f) * 1;
            }
        }
    }
}
