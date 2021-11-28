using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This is so that it should find the Text component 
using UnityEngine.SceneManagement;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler instance;

    //Index into these lists are score IDs
    public Text[] texts;

    public bool showScores = true;

    public Transform[] holders;
    public Image[] bases;
    public Image[] guns;

    //player 1 has index 1, max 8 players

    // Start is called before the first frame update

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
        {
            texts[i].color = settingsHandler.instance.colors[i];
            bases[i].color = settingsHandler.instance.colors[i];
            guns[i].color = settingsHandler.instance.colors[i];
        }
        showScores = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("main");
        }

        if (Input.GetKeyDown("t"))
        {
            SceneManager.LoadScene("main");
        }

        //move to pause screen
        if (Input.GetKeyDown("p"))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
        {
            texts[i].text = settingsHandler.instance.scores[i].ToString();
        }

        if (showScores == true)
        {
            for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
            {
                holders[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i <= settingsHandler.instance.tankCount; i++)
            {
                holders[i].gameObject.SetActive(false);
            }
        }

    }

    public void AddScore(int player, int score = 1)
    {
        settingsHandler.instance.scores[player] += score;
    }
}
