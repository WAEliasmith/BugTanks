using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This is so that it should find the Text component 

public class ScoreHandler : MonoBehaviour
{
    public PvPDirector director;

    public Color[] colors;
    //Index into these lists are score IDs
    public Text[] texts;

    public int[] scores;

    public int tankCount;

    public Transform[] holders;
    public Image[] bases;
    public Image[] guns;

    //player 1 has index 1, max 8 players

    // Start is called before the first frame update
    void Awake()
    {
        colors = director.colors;
        tankCount = director.numTanks;

        for (int i = 1; i <= tankCount; i++)
        {
            holders[i].gameObject.SetActive(true); //comment out later
            texts[i].color = colors[i];
            bases[i].color = colors[i];
            guns[i].color = colors[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i <= tankCount; i++)
        {
            texts[i].text = scores[i].ToString();
        }
    }

    void FixedUpdate()
    {
        if (director.time == 10)
        {
            for (int i = 1; i <= tankCount; i++)
            {
                holders[i].gameObject.SetActive(false);
            }
        }
        else if (director.time == -1)
        {
            for (int i = 1; i <= tankCount; i++)
            {
                holders[i].gameObject.SetActive(true);
            }
        }
    }

    public void AddScore(int player, int score = 1)
    {
        scores[player] += score;
    }
}
