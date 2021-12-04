using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeSurvived : MonoBehaviour
{
    float timer = 0;
    public Text crispText;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        crispText.text = "Time Survived: " + timer.ToString("F2");

    }
}
