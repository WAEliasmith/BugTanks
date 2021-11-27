using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvPDirector : MonoBehaviour
{
    public float time = 0;
    public float dodgeTime = Mathf.Infinity;
    public float dodgeTime2 = Mathf.Infinity;
    public float awardTime = Mathf.Infinity;
    public int survivorPointsAwarded = 0;

    public float dodgeDuration = 200f;
    public float dodgeDuration2 = 100f;

    public int survivorPoints = 3;

    public float noWallsNear;
    public float noPlayersNear;
    private float minSpawnDist;
    private float maxSpawnDist;
    public GameObject PlayerTank = null;
    public GameObject AITank = null;

    public GameObject[] tanks;

    // Start is called before the first frame update
    void Start()
    {
        ScoreHandler.instance.showScores = false;
        Time.timeScale = 1;
        float range = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize.y;
        minSpawnDist = range * 0.25f;
        maxSpawnDist = range * 0.5f;
        noPlayersNear = 3.2f;

        for (int i = 1; i <= ScoreHandler.instance.tankCount; i++)
        {
            if (i <= ScoreHandler.instance.numPlayers)
            {
                //create a player
                CreateTank(PlayerTank, ScoreHandler.instance.colors[i], i);
            }
            else
            {
                //create an AI
                CreateTank(AITank, ScoreHandler.instance.colors[i], i);
            }

        }
    }

    void FixedUpdate()
    {
        if (time != -1)
        {
            time++;
        }
        if (time % 10 == 0)
        {
            int deadCount = 0;
            for (int i = 1; i <= ScoreHandler.instance.tankCount; i++)
            {
                if (tanks[i].activeSelf == false)
                {
                    deadCount++;
                }
            }
            if (deadCount >= (ScoreHandler.instance.tankCount - 1) && dodgeTime == Mathf.Infinity)
            {
                dodgeTime = time;
                Debug.Log("dodgeTime starts");
                awardTime = dodgeTime + (dodgeDuration - 10) / survivorPoints;

            }
            if (deadCount == ScoreHandler.instance.tankCount && dodgeTime2 == Mathf.Infinity)
            {
                dodgeTime2 = time;
                dodgeTime = time;
                Debug.Log("dodgeTime2 starts");
            }
        }

        if (time > awardTime)
        {
            //currently dodging
            awardTime += (dodgeDuration - 10) / survivorPoints;
            survivorPointsAwarded += 1;
            Debug.Log("finna award");
            if (survivorPointsAwarded <= survivorPoints)
            {
                for (int i = 1; i <= ScoreHandler.instance.tankCount; i++)
                {
                    if (tanks[i].activeSelf == true)
                    {
                        Debug.Log("award completed to " + i);
                        ScoreHandler.instance.AddScore(i);
                    }
                }
            }
            //award points
        }

        if (time > dodgeTime + dodgeDuration)
        {
            time = -1;
            //pause Game
            ScoreHandler.instance.showScores = true;

            StartCoroutine(updateGraphDelayed());
            Time.timeScale = 0;
        }
        else if (time > dodgeTime2 + dodgeDuration2)
        {
            time = -1;
            //game ends with no survivor
            Debug.Log("game ends with no survivor");
            //pause Game
            ScoreHandler.instance.showScores = true;

            StartCoroutine(updateGraphDelayed());
            Time.timeScale = 0;
        }
    }



    IEnumerator updateGraphDelayed()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("main");
    }

    void CreateTank(GameObject tankToSpawn, Color color, int scoreNumber)
    {
        int wallLayerMask = 1 << 8 | 1 << 9;
        int playerLayerMask = 1 << 6 | 1 << 7;
        for (int j = 0; j < 999; j++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            Vector3 position = (Vector3)direction * Random.Range(minSpawnDist, maxSpawnDist);
            //see if position is on wall
            Collider2D wall = Physics2D.OverlapBox(transform.position + position, new Vector2(noWallsNear, noWallsNear), 0f, wallLayerMask);
            Collider2D player = Physics2D.OverlapBox(transform.position + position, new Vector2(noPlayersNear, noPlayersNear), 0f, playerLayerMask);

            if (wall == null && player == null)
            {
                tanks[scoreNumber] = Instantiate(tankToSpawn, transform.position + position, Quaternion.identity);
                tanks[scoreNumber].GetComponent<Gun>().tankColor = color;
                tanks[scoreNumber].GetComponent<Gun>().scoreNumber = scoreNumber;
                tanks[scoreNumber].GetComponent<MoveTank>().innerAngle = Random.Range(0, 360);
                return;
            }
        }
        Debug.Log("FATAL ERROR: Tank could not be spawned");
        return;
    }
}
