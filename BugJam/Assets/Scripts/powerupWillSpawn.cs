using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupWillSpawn : MonoBehaviour
{
    public float timer = 60;
    public float timerLeft;

    public string powerup = "none";
    public GameObject powerUp;

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
            GameObject currPowerup = Instantiate(powerUp, transform.position, Quaternion.identity);
            currPowerup.GetComponent<Powerup>().powerup = powerup;
            Destroy(gameObject);
        }
    }
}
