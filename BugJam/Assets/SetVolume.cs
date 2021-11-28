using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string volTitle = "MusicVol";

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(volTitle, Mathf.Log10(sliderValue) * 20);
    }
}
