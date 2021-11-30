using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVariety : MonoBehaviour
{
    public AudioSource sound = null;
    public float soundTop = 1f;
    public float soundBottom = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        if (sound)
        {
            sound.pitch = Random.Range(soundBottom, soundTop);
            sound.Play();
        }
    }
}
