using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chromaticButton : MonoBehaviour
{
    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.chrome)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }
}
