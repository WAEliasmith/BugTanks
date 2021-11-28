using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalButton : MonoBehaviour
{
    public Text survivalText;

    // Update is called once per frame
    void Update()
    {
        survivalText.text = "" + settingsHandler.instance.pointsForSurvival;
    }
}
