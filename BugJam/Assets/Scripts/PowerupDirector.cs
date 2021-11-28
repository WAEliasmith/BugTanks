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

    public string[] enabledPowerups;

    public GameObject powerupPrefab = null;

    public PvPDirector pvp;

    public float rangeMod = 1;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        if (pvp)
        {
            initialTimeToSpawn -= settingsHandler.instance.tankCount * 8f;
        }
        range = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize.y * rangeMod;
        timeToSpawn = initialTimeToSpawn;
        timeToSpawnLeft = timeToSpawn;
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        r += rateIncrease;

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
                powerup.GetComponent<powerupWillSpawn>().powerup = enabledPowerups[Random.Range(0, enabledPowerups.Length)];
                return;
            }
        }
    }
}
