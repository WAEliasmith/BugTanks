using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupWillSpawnVisuals : MonoBehaviour
{
    public powerupWillSpawn body;
    // Update is called once per frame
    void Update()
    {

        GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1 - (body.timerLeft / body.timer);
        float stretch = (body.timerLeft / body.timer) + 0.01f;
        GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightInnerRadius = stretch;
        GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = stretch + 0.2f;
    }
}
