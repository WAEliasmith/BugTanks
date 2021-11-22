using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDirector : MonoBehaviour
{
    public float timeToSpawn;
    public float r;
    public float rateIncrease = 0.1f;
    public float timeToSpawnLeft;
    public float initialTimeToSpawn = 100;

    public string[] enabledPowerups;

    public GameObject powerupPrefab = null;

    public float rangeMod = 1;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
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
            timeToSpawn = initialTimeToSpawn / r;
            timeToSpawnLeft = timeToSpawn;
            CreatePowerup();
        }
    }

    void CreatePowerup()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 position = (Vector3)direction * Random.Range(0f, range);

        GameObject powerup = Instantiate(powerupPrefab, transform.position + position, Quaternion.identity);
        powerup.GetComponent<powerupWillSpawn>().powerup = enabledPowerups[Random.Range(0, enabledPowerups.Length)];
    }
}
