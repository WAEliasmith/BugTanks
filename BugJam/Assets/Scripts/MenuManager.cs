using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

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

            //create level select
            CreateLevelSelect();

            LoadScreen(Controls, false);
            LoadScreen(Dictionary, false);
            LoadScreen(settingsScreen, false);
            LoadScreen(singlePlayerScreen, false);
            LoadScreen(multiPlayerScreen, false);
            LoadScreen(battleScreen, false);
            LoadScreen(campaignScreen, false);
            LoadScreen(pauseScreen, false);
            LoadScreen(watchScreen, false);
            LoadScreen(titleScreen);
        }
    }

    public int currMusic = -1;
    public AudioSource menuMusic;
    public AudioSource campaignMusic;

    public AudioSource bounce;
    public AudioSource click;


    public string[] levelNames;

    public GameObject levelSelectButton = null;

    public float menuTimeScaleMult = 1f;

    public bool pause = false;

    private float buttonSpacing = 150f;

    public int currentLevel = 0;

    public bool watch = false;

    public bool[] powerupToggle;

    public string[] allPowerups;

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            if (currMusic != 0)
            {
                menuMusic.Play();
                campaignMusic.Stop();
                currMusic = 0;
            }
        }
        else
        {
            if (currMusic != 1)
            {
                currMusic = 1;
                campaignMusic.Play();
                menuMusic.Stop();
            }
        }
        if (SceneManager.GetActiveScene().name != "TitleScreen")
        {
            if (Input.GetKeyDown("p") || Input.GetKeyDown("escape"))
            {
                TogglePause();
            }
        }
        else
        {
            pause = false;
        }
        if (pause == false)
        {
            Time.timeScale = menuTimeScaleMult;
        }
        else
        {
            Time.timeScale = 0;
        }
    }


    public GameObject[] titleScreen;
    public GameObject[] settingsScreen;

    public GameObject[] singlePlayerScreen;
    public GameObject[] multiPlayerScreen;

    public GameObject[] battleScreen;
    public GameObject[] campaignScreen;

    public GameObject[] pauseScreen;

    public GameObject[] watchScreen;

    public GameObject[] Dictionary;
    public GameObject[] Controls;

    public void SinglePlayer()
    {
        settingsHandler.instance.numPlayers = 1;
        ChangeScreen(titleScreen, singlePlayerScreen);
    }

    public void Multiplayer()
    {
        settingsHandler.instance.numPlayers = 2;
        ChangeScreen(titleScreen, multiPlayerScreen);
    }

    public void Settings()
    {
        LoadScreen(pauseScreen, false);
        ChangeScreen(titleScreen, settingsScreen);
    }

    public void Watch()
    {
        if (watch == false)
        {
            watch = true;
            ChangeScreen(titleScreen, watchScreen);
        }
        else
        {
            watch = false;
            ChangeScreen(watchScreen, titleScreen);
        }
    }

    public void DictionaryButton()
    {
        ChangeScreen(settingsScreen, Dictionary);
    }

    public void ControlsButton()
    {
        ChangeScreen(settingsScreen, Controls);
    }

    public void BackToMenu()
    {
        Noise2();
        LoadScreen(Dictionary, false);
        LoadScreen(Controls, false);
        LoadScreen(settingsScreen, false);
        LoadScreen(singlePlayerScreen, false);
        LoadScreen(multiPlayerScreen, false);
        LoadScreen(battleScreen, false);
        LoadScreen(campaignScreen, false);
        LoadScreen(pauseScreen, false);
        LoadScreen(watchScreen, false);
        if (pause == false)
        {
            LoadScreen(titleScreen);
        }
        else
        {
            LoadScreen(pauseScreen);
        }
    }

    public void TogglePause()
    {

        if (pause == false)
        {
            pause = true;
            LoadScreen(settingsScreen, false);
            LoadScreen(singlePlayerScreen, false);
            LoadScreen(multiPlayerScreen, false);
            LoadScreen(battleScreen, false);
            LoadScreen(campaignScreen, false);
            LoadScreen(titleScreen, false);
            LoadScreen(pauseScreen);
            LoadScreen(watchScreen, false);
        }
        else
        {
            pause = false;
            LoadScreen(settingsScreen, false);
            LoadScreen(pauseScreen, false);

        }

    }

    public void loadMenu()
    {
        Noise2();
        pause = false;
        LoadScreen(titleScreen);
        LoadScreen(pauseScreen, false);
        SceneManager.LoadScene("TitleScreen");
    }

    public void restartRound()
    {
        pause = false;
        LoadScreen(pauseScreen, false);
        Noise2();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Campaign()
    {
        LoadScreen(multiPlayerScreen, false);
        ChangeScreen(singlePlayerScreen, campaignScreen);
    }

    public void BattleArena()
    {
        LoadScreen(multiPlayerScreen, false);
        ChangeScreen(singlePlayerScreen, battleScreen);
    }

    public void ChangeScreen(GameObject[] oldScreen, GameObject[] newScreen)
    {
        Noise2();
        LoadScreen(oldScreen, false);
        LoadScreen(newScreen);
    }

    public void LoadScreen(GameObject[] screen, bool load = true)
    {
        for (int i = 0; i < screen.Length; i++)
        {
            screen[i].SetActive(load);
        }
    }

    void CreateLevelSelect()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject b = Instantiate(levelSelectButton, transform.position, Quaternion.identity);
                b.transform.SetParent(gameObject.transform, false);

                b.transform.localPosition = new Vector3(-3.5f * buttonSpacing, -100 + 1.5f * buttonSpacing, 0f) + new Vector3(j * buttonSpacing, -i * buttonSpacing, 1.2f);
                b.transform.GetChild(0).GetComponent<Text>().text = "" + (i * 8 + j + 1);
                b.transform.localScale = new Vector3(1f, 1f, 1f);

                StartCoroutine(setButton(b, i, j));
                campaignScreen[2 + i * 8 + j] = b;
            }
        }
    }

    IEnumerator setButton(GameObject b, int i, int j)
    {
        b.GetComponent<Button>().onClick.AddListener(delegate { goToLevel(i * 8 + j); });
        yield return null;
    }


    public void goToLevel(int level)
    {
        settingsHandler.instance.pvp = false;
        LoadScreen(campaignScreen, false);
        currentLevel = level;
        SceneManager.LoadScene(levelNames[level]);
    }

    public void goToNextLevel()
    {
        if (currentLevel < 24)
        {
            goToLevel(currentLevel + 1);
        }
    }

    public void togglePowerup(int index)
    {
        powerupToggle[index] = !powerupToggle[index];
        Noise();
    }

    public void startArena()
    {
        LoadScreen(battleScreen, false);


        List<string> powerups = new List<string>();
        for (int i = 0; i < powerupToggle.Length; i++)
        {
            if (powerupToggle[i])
            {
                powerups.Add(allPowerups[i]);
            }
        }
        settingsHandler.instance.enabledPowerups = powerups;

        loadArena();
    }

    public void loadArena()
    {
        settingsHandler.instance.pvp = true;

        if (settingsHandler.instance.pvpMapSize == 1)
        {
            SceneManager.LoadScene("mainsmall");
        }
        else if (settingsHandler.instance.pvpMapSize == 2)
        {
            SceneManager.LoadScene("mainmedium");
        }
        else if (settingsHandler.instance.pvpMapSize == 3)
        {
            SceneManager.LoadScene("main"); //large
        }
        else if (settingsHandler.instance.pvpMapSize == 4)
        {
            SceneManager.LoadScene("mainmassive");
        }
    }
    public void Noise()
    {
        bounce.pitch = Random.Range(0.8f, 1f);
        bounce.Play();
    }

    public void Noise2()
    {
        click.pitch = Random.Range(0.8f, 1f);
        click.Play();
    }

    public void toggleArenaSize()
    {
        Noise();
        settingsHandler.instance.pvpMapSize++;
        if (settingsHandler.instance.pvpMapSize > 4)
        {
            settingsHandler.instance.pvpMapSize = 1;
        }
    }

    public void toggleCrisp()
    {
        Noise();
        settingsHandler.instance.crisp = !settingsHandler.instance.crisp;
    }

    public void toggleCamera()
    {
        Noise();
        settingsHandler.instance.cameraFollow = !settingsHandler.instance.cameraFollow;
    }

    public void togglePlayers()
    {
        Noise();
        settingsHandler.instance.tankCount++;
        if (settingsHandler.instance.tankCount > 8)
        {
            settingsHandler.instance.tankCount = 2;
        }
    }

    public void togglePointsForSurvival()
    {
        Noise();
        settingsHandler.instance.pointsForSurvival++;
        if (settingsHandler.instance.pointsForSurvival > 10)
        {
            settingsHandler.instance.pointsForSurvival = 0;
        }
        if (settingsHandler.instance.pointsForSurvival > 4 && settingsHandler.instance.pointsForSurvival % 2 != 0)
        {
            settingsHandler.instance.pointsForSurvival++;
        }
    }

}
