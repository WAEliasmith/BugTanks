using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helperbutton : MonoBehaviour
{
    public Text helperText;

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.turnLockEnabled)
        {
            helperText.text = "Limit Rotation When Glitching";
        }
        else
        {
            helperText.text = "Disabled";
        }
    }
}
