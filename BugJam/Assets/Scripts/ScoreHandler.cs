using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This is so that it should find the Text component 

public class ScoreHandler : MonoBehaviour
{
    public StartGame director;

    public Color[] colors;
    //Index into these lists are score IDs
    public Text[] texts;

    public int[] scores;

    public int tankCount;

    public Transform[] holders;
    public Image[] tanks;
    public Image[] guns;

    //player 1 has index 1, max 8 players

    // Start is called before the first frame update
    void Start()
    {
        colors = director.colors;

        for (int i = 1; i <= tankCount; i++)
        {
            texts[i].enabled = true;
            texts[i].color = colors[i];
            tanks[i].color = colors[i];
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

    public void AddScore(int player)
    {
        scores[player] += 1;
    }
}
