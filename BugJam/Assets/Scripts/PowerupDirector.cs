using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDirector : MonoBehaviour
{
    public float timeToSpawn;
    public float r;
    public float rPlayerIncrease = 0.1f;
    public float rateIncrease = 0.1f;
    public float timeToSpawnLeft;
    public float initialTimeToSpawn = 100;
    public float noWallsNear = 1f;
    public float extraInitialWait = 20f;

    public List<string> enabledPowerups;

    public GameObject powerupPrefab = null;

    public PvPDirector pvp;

    public float rangeMod = 1;
    private float range;
    public float maxRate = 4;

    public bool troll = false;
    public bool troll2 = false;
    // Start is called before the first frame update
    void Start()
    {
        if (pvp)
        {
            enabledPowerups = settingsHandler.instance.enabledPowerups;
            initialTimeToSpawn -= settingsHandler.instance.tankCount * 7f;
        }
        range = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize.y * rangeMod;
        timeToSpawn = initialTimeToSpawn;
        timeToSpawnLeft = timeToSpawn;
        timeToSpawnLeft += extraInitialWait;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (r < maxRate)
        {
            r += rateIncrease;
        }

        timeToSpawnLeft--;
        if (timeToSpawnLeft <= 0)
        {
            if (pvp)
            {
                timeToSpawn = initialTimeToSpawn / (r + pvp.aliveCount * rPlayerIncrease);
            }
            else
            {
                timeToSpawn = initialTimeToSpawn / r;
            }

            timeToSpawnLeft = timeToSpawn;
            CreatePowerup();
        }
    }

    void CreatePowerup()
    {
        int wallLayerMask = 1 << 8 | 1 << 9;
        for (int i = 0; i < 50; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 position = (Vector3)direction * Random.Range(0f, range / 2);
            //see if position is on wall
            Collider2D box = Physics2D.OverlapBox(transform.position + position, new Vector2(noWallsNear, noWallsNear), 0f, wallLayerMask);
            if (box == null)
            {
                GameObject powerup = Instantiate(powerupPrefab, transform.position + position, Quaternion.identity);
                if (troll == false)
                {
                    if (enabledPowerups.Count > 0)
                    {
                        powerup.GetComponent<powerupWillSpawn>().powerup = enabledPowerups[Random.Range(0, enabledPowerups.Count)];
                    }
                }
                else
                {
                    if (troll2 == false)
                    {
                        powerup.GetComponent<MoveTank>().innerAngle = Random.Range(0, 360);
                        powerup.GetComponent<Gun>().tankColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                    }
                }
                return;
            }
        }
    }
}
