using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public int numPlayers = 1;
    public int numTanks = 8;
    public Color[] colors;

    public float noWallsNear;
    public float noPlayersNear;
    private float minSpawnDist;
    private float maxSpawnDist;
    public GameObject PlayerTank = null;
    public GameObject AITank = null;

    // Start is called before the first frame update
    void Start()
    {
        float range = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize.y;
        minSpawnDist = range * 0.25f;
        maxSpawnDist = range * 0.5f;

        for (int i = 1; i <= numTanks; i++)
        {
            if (i <= numPlayers)
            {
                //create a player
                CreateTank(PlayerTank, colors[i], i);
            }
            else
            {
                //create an AI
                CreateTank(AITank, colors[i], i);
            }

        }
    }

    void CreateTank(GameObject tankToSpawn, Color color, int scoreNumber)
    {
        int wallLayerMask = 1 << 8 | 1 << 9;
        int playerLayerMask = 1 << 6 | 1 << 7;
        for (int i = 0; i < 999; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 position = (Vector3)direction * Random.Range(minSpawnDist, maxSpawnDist);
            //see if position is on wall
            Collider2D wall = Physics2D.OverlapBox(transform.position + position, new Vector2(noWallsNear, noWallsNear), 0f, wallLayerMask);
            Collider2D player = Physics2D.OverlapBox(transform.position + position, new Vector2(noWallsNear, noWallsNear), 0f, wallLayerMask);

            if (wall == null && player == null)
            {
                GameObject tank = Instantiate(tankToSpawn, transform.position + position, Quaternion.identity);
                tank.GetComponent<Gun>().tankColor = color;
                tank.GetComponent<Gun>().scoreNumber = scoreNumber;
                tank.GetComponent<MoveTank>().rb.rotation = Random.Range(0, 360);
                return;
            }
        }
        Debug.Log("FATAL ERROR: Tank could not be spawned");
        return;
    }
}
