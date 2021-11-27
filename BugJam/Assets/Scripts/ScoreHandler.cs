using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This is so that it should find the Text component 
using UnityEngine.SceneManagement;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler instance;

    public PvPDirector director;

    public Color[] colors;
    //Index into these lists are score IDs
    public Text[] texts;

    public int[] scores;

    public int tankCount = 8;
    public int numPlayers = 1;
    public bool showScores = true;

    public Transform[] holders;
    public Image[] bases;
    public Image[] guns;

    //player 1 has index 1, max 8 players

    // Start is called before the first frame update

    void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);

        for (int i = 1; i <= tankCount; i++)
        {
            holders[i].gameObject.SetActive(true); //comment out later
            texts[i].color = colors[i];
            bases[i].color = colors[i];
            guns[i].color = colors[i];
        }
    }

    void Start()
    {
        showScores = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
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

        for (int i = 1; i <= tankCount; i++)
        {
            texts[i].text = scores[i].ToString();
        }

        if (showScores == true)
        {
            for (int i = 1; i <= tankCount; i++)
            {
                holders[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 1; i <= tankCount; i++)
            {
                holders[i].gameObject.SetActive(false);
            }
        }

    }

    public void AddScore(int player, int score = 1)
    {
        scores[player] += score;
    }
}
