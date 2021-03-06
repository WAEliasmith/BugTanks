using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupWillSpawn : MonoBehaviour
{
    public float timer = 60;
    public float timerLeft;

    public string powerup = "none";
    public GameObject powerUp;

    public bool troll = false;

    public bool powerUpDespawns = true;

    public bool troll3 = false;

    public GameObject powerupParent = null;

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
            if (!troll)
            {
                currPowerup.GetComponent<Powerup>().powerup = powerup;
                if (powerUpDespawns == false)
                {
                    currPowerup.GetComponent<Powerup>().maxLife = 999999;
                }
                if (powerupParent != null)
                {
                    currPowerup.GetComponent<Powerup>().powerupParent = powerupParent;
                }
            }
            else
            {
                if (currPowerup.transform.Find("Turret"))
                {
                    //spawning turret
                    gameObject.transform.Find("Turret").GetComponent<MoveTank>().powerupParent = powerupParent;
                }
                else
                {
                    //spawning tank 
                    currPowerup.GetComponent<MoveTank>().powerupParent = powerupParent;
                }
            }
            if (troll3 == true)
            {
                currPowerup.GetComponent<bullet>().color = new Color(Random.Range(0f, 1f), Random.Range(0.4f, 1f), Random.Range(0f, 1f));
            }
            Destroy(gameObject);
        }
    }
}
