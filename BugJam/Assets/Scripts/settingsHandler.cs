using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class settingsHandler : MonoBehaviour
{
    public static settingsHandler instance;

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

    public float pvpMapSize = 2;

    public bool crisp = false;

    public int[] scores;
    public Color[] colors;

    public bool pvp = false;
    public int tankCount = 8;
    public int numPlayers = 1;

    public int pointsForSurvival = 3;

    public List<string> enabledPowerups;

    public bool cameraFollow = true;

    // Update is called once per frame
    void Update()
    {
        Time.fixedDeltaTime = 1f / 60f;
    }

    public void AddScore(int player, int score = 1)
    {
        if (pvp)
        {
            ScoreHandler.instance.AddScore(player, score);
        }
    }
}
