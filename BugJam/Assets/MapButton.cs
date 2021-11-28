using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    public Text mapText;

    // Update is called once per frame
    void Update()
    {
        if (settingsHandler.instance.pvpMapSize == 1)
        {
            mapText.text = "Small";
        }
        else if (settingsHandler.instance.pvpMapSize == 2)
        {
            mapText.text = "Medium";
        }
        else if (settingsHandler.instance.pvpMapSize == 3)
        {
            mapText.text = "Large";
        }
        else if (settingsHandler.instance.pvpMapSize == 4)
        {
            mapText.text = "Massive";
        }
    }
}
