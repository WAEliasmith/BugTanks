using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersButton : MonoBehaviour
{
    public Text playersText;

    // Update is called once per frame
    void Update()
    {
        playersText.text = "" + settingsHandler.instance.tankCount;
    }
}
