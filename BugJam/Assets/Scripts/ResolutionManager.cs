using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public int width;
    public int height;
    public void SetRes()
    {
        Screen.SetResolution(width, height, false);
    }
    public void SetWidth(int setwidth)
    {
        width = setwidth;
    }
    public void SetHeight(int setheight)
    {
        height = setheight;
    }
}
