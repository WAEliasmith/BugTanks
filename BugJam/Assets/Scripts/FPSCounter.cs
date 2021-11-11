using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;

    public float update = 0.5f;
    private float lastFrameRate;
    private float updateLeft;

    void Start(){
        lastFrameRate = (1f / Time.unscaledDeltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        updateLeft -= Time.deltaTime;
        
        float fps = (1f / Time.unscaledDeltaTime);
        lastFrameRate = lastFrameRate*0.95f+fps*0.05f;
        if (updateLeft < 0){
            updateLeft = update;
            fpsText.text = ((int)lastFrameRate).ToString();
        }
    }
}
