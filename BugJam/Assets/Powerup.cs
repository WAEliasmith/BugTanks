using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string powerup = "";
    public int maxLife = 500;

    private int lifeLeft;

    // Start is called before the first frame update
    void Start()
    {
        lifeLeft = maxLife;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (lifeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Collected()
    {
        Destroy(gameObject);
    }
}
