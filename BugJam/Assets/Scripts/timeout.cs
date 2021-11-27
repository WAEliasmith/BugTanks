using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeout : MonoBehaviour
{
    public float life = 100;
    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        life--;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
