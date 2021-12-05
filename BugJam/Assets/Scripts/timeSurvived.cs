using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeSurvived : MonoBehaviour
{
    float timer = 0;
    public Text crispText;

    public PvEDirector director = null;


    // Update is called once per frame
    void Update()
    {
        if (director.playerCount > 0)
        {
            timer += Time.deltaTime;
        }
        crispText.text = "Time Survived: " + timer.ToString("F2");
    }
}
