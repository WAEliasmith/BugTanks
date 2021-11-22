using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupWillSpawn : MonoBehaviour
{
    public int timer = 60;
    private int timerLeft;

    public GameObject powerUp = null;

    // Start is called before the first frame update
    void Start()
    {
        timerLeft = timer;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        timerLeft--;
        if (timerLeft == 0)
        {
            Instantiate(powerUp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
