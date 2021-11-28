using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public Text crispText;

    // Update is called once per frame
    void Update()
    {
        if (settingsHandler.instance.cameraFollow == true)
        {
            crispText.text = "Follow";
        }
        else
        {
            crispText.text = "Fixed";
        }
    }
}
