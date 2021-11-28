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

            BackToMenu();
            //bounce.Play();
            music.Play();
        }
    }

    public AudioSource music;
    public AudioSource bounce;

    public string[] levelNames;

    public GameObject levelSelectButton = null;

    public float menuTimeScaleMult = 1f;

    public bool pause = false;

    private float buttonSpacing = 150f;

    public int currentLevel = 0;

    public bool watch = false;

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "TitleScreen")
        {
            if (Input.GetKeyDown("p") || Input.GetKeyDown("escape"))
            {
                TogglePause();
            }
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

    public void BackToMenu()
    {
        LoadScreen(settingsScreen, false);
        LoadScreen(singlePlayerScreen, false);
        LoadScreen(multiPlayerScreen, false);
        LoadScreen(battleScreen, false);
        LoadScreen(campaignScreen, false);
        LoadScreen(pauseScreen, false);
        LoadScreen(watchScreen, false);
        LoadScreen(titleScreen);
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
            LoadScreen(pauseScreen, false);

        }

    }

    public void loadMenu()
    {
        pause = false;
        LoadScreen(titleScreen);
        LoadScreen(pauseScreen, false);
        SceneManager.LoadScene("TitleScreen");
    }

    public void restartRound()
    {
        pause = false;
        LoadScreen(pauseScreen, false);


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
        for (int i = 0; i < 4; i++)
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
        LoadScreen(campaignScreen, false);
        currentLevel = level;
        SceneManager.LoadScene(levelNames[level]);
    }

    public void goToNextLevel()
    {
        if (currentLevel < 32)
        {
            goToLevel(currentLevel + 1);
        }
    }

}
