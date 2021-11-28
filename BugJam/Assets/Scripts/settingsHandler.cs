using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class settingsHandler : MonoBehaviour
{
    public float pvpMapSize = 1;

    public static settingsHandler instance;

    public bool crisp;

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
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

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
