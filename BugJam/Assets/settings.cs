using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    public float pvpMapSize = 1;

    public static settings instance;

    public int[] scores;
    public Color[] colors;

    public bool pvp = false;
    public int tankCount = 8;
    public int numPlayers = 1;

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
    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int player, int score = 1)
    {
        if (pvp)
        {
            ScoreHandler.instance.AddScore(player, score);
        }
    }
}
