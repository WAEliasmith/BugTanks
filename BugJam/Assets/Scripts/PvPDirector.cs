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
    public int aliveCount;

    public GameObject[] tanks;


    // Start is called before the first frame update
    void Start()
    {
        aliveCount = settingsHandler.instance.tankCount;
        ScoreHandler.instance.showScores = false;
        MenuManager.instance.menuTimeScaleMult = 1;
        float range = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize.y;
        minSpawnDist = range * 0.25f;
        maxSpawnDist = range * 0.5f;
        noPlayersNear = 3.2f;

        for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
        {
            if (i <= settingsHandler.instance.numPlayers)
            {
                int controls = 0;

                if (settingsHandler.instance.numPlayers > 1)
                {
                    if (i == 1)
                    {
                        controls = 1;
                    }
                    else if (i == 2)
                    {
                        controls = 2;
                    }
                }
                //create a player
                CreateTank(PlayerTank, settingsHandler.instance.colors[i], i, controls);
            }
            else
            {
                //create an AI
                CreateTank(AITank, settingsHandler.instance.colors[i], i);
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
            for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
            {
                if (tanks[i].activeSelf == false)
                {
                    deadCount++;
                }
            }
            if (deadCount >= (settingsHandler.instance.tankCount - 1) && dodgeTime == Mathf.Infinity)
            {
                dodgeTime = time;
                awardTime = dodgeTime + (dodgeDuration - 10) / survivorPoints;

            }
            if (deadCount == settingsHandler.instance.tankCount && dodgeTime2 == Mathf.Infinity)
            {
                dodgeTime2 = time;
                dodgeTime = time;
            }
            aliveCount = settingsHandler.instance.tankCount - deadCount;
        }

        if (time > awardTime)
        {
            //currently dodging
            awardTime += (dodgeDuration - 10) / survivorPoints;
            survivorPointsAwarded += 1;
            if (survivorPointsAwarded <= survivorPoints)
            {
                for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
                {
                    if (tanks[i].activeSelf == true)
                    {
                        settingsHandler.instance.AddScore(i);
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

            StartCoroutine(delay());
            MenuManager.instance.menuTimeScaleMult = 0;
        }
        else if (time > dodgeTime2 + dodgeDuration2)
        {
            time = -1;
            //game ends with no survivor
            //pause Game
            ScoreHandler.instance.showScores = true;

            StartCoroutine(delay());
            MenuManager.instance.menuTimeScaleMult = 0;
        }
    }

    IEnumerator delay(float delayTime = 1.5f)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        if (MenuManager.instance.pause == false)
        {
            SceneManager.LoadScene("main");
        }
        else
        {
            StartCoroutine(delay(0.5f));
        }
    }

    void CreateTank(GameObject tankToSpawn, Color color, int scoreNumber, int controls = -1)
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
                if (controls != -1)
                {
                    tanks[scoreNumber].GetComponent<PlayerController>().playerControlsNumber = controls;

                }
                return;
            }
        }
        Debug.Log("FATAL ERROR: Tank could not be spawned");
        return;
    }
}
