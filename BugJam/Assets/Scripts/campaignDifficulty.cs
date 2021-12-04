using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class campaignDifficulty : MonoBehaviour
{
    public Text diffText;

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.difficultyLevelHearts == -1)
        {
            diffText.text = "No-Hit";
        }
        else if (MenuManager.instance.difficultyLevelHearts == 0)
        {
            diffText.text = "Hard";
        }
        else if (MenuManager.instance.difficultyLevelHearts == 1)
        {
            diffText.text = "Normal";
        }
        else if (MenuManager.instance.difficultyLevelHearts == 2)
        {
            diffText.text = "Easy";
        }
    }
}
