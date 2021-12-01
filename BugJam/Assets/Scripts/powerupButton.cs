using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerupButton : MonoBehaviour
{
    public int toLookFor;
    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.powerupToggle[toLookFor])
        {
            image.color = new Color(0.5859375f, 0.2734375f, 1f, 1f);
        }
        else
        {
            image.color = new Color(0.5859375f, 0.2734375f, 1f, 0.5f);
        }
    }
}
