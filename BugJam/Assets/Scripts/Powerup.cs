using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameObject collectedExplosion = null;
    public string powerup = "";
    public int maxLife = 500;

    public int lifeLeft;

    // Start is called before the first frame update
    void Start()
    {
        lifeLeft = maxLife;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        lifeLeft--;
        if (lifeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Collected()
    {
        Instantiate(collectedExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
