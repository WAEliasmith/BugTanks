using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuckoff : MonoBehaviour
{
    public float time = 500;
    public float speed = 0;
    // Update is called once per frame
    void Update()
    {
        time--;
        if (time <= 0)
        {
            speed += 0.03f;
        }

        transform.position += new Vector3(0f, speed, 0f);
    }
}
