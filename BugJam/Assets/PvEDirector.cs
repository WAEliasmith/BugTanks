using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvEDirector : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> players;

    public string nextLevel = "level1";

    //private int time;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        enemies = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EnemyToKill"))
        {
            enemies.Add(obj);
        }
        players = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CameraTarget"))
        {
            players.Add(obj);
            if (settings.instance.numPlayers == 1)
            {
                //disable 2 player controllers
                if (obj.name == "Player1" || obj.name == "Player2")
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                //disable 1 player controller
                if (obj.name == "Player0")
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //time++;
        //if (time % 10 == 0)
        //{
        int aliveCount = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].activeSelf == true)
            {
                aliveCount++;
            }
        }
        if (aliveCount == 0)
        {
            //go to next level
            StartCoroutine(delay());
            Time.timeScale = 0;
        }
        int playerCount = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].activeSelf == true)
            {
                playerCount++;
            }
        }

        if (playerCount == 0)
        {
            //go to current level
            StartCoroutine(delay(true));
            Time.timeScale = 0;
        }
        //}
    }

    IEnumerator delay(bool fail = false)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (fail)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
