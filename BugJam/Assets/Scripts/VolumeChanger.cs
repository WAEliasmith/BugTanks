using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    public GameObject peepee = null;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Rendering.VolumeProfile volumeProfile = peepee.GetComponent<UnityEngine.Rendering.Volume>()?.profile;


    }

    // Update is called once per frame
    void Update()
    {

    }
}
