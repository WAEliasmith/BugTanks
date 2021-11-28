using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrispButton : MonoBehaviour
{
    public Text crispText;

    // Update is called once per frame
    void Update()
    {
        if (settingsHandler.instance.crisp == true)
        {
            crispText.text = "Safe";
        }
        else
        {
            crispText.text = "Bouncy";
        }
    }
}
