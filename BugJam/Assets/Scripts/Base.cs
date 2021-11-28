using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public GameObject turret = null;

    // Update is called once per frame
    void Update()
    {
        if (turret.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
    }
}
