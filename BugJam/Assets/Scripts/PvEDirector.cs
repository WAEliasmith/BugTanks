using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PvEDirector : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> players;

    //private int time;
    // Start is called before the first frame update
    void Start()
    {
        MenuManager.instance.menuTimeScaleMult = 1;

        enemies = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EnemyToKill"))
        {
            enemies.Add(obj);
        }
        players = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CameraTarget"))
        {
            players.Add(obj);
            if (settingsHandler.instance.numPlayers == 1)
            {
                //disable 2 player controllers and hearts
                if (obj.name == "Player1" || obj.name == "Player2")
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                //disable 1 player controller and hearts
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
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i])
                {
                    players[i].GetComponent<Gun>().hbox.iFrames = 10001f;
                }
            }
            StartCoroutine(delay());
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
        }
        //}
    }

    IEnumerator delay(bool fail = false, float delayTime = 1.5f)
    {
        if (MenuManager.instance.pause == false)
        {
            yield return new WaitForSecondsRealtime(delayTime);
            if (fail)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                MenuManager.instance.goToNextLevel();
            }
        }
        else
        {
            StartCoroutine(delay(fail, 0.5f));
        }

    }
}
