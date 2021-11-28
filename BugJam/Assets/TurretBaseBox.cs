using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseBox : MonoBehaviour
{
    public GameObject turret = null;

    // Update is called once per frame
    void Update()
    {
        transform.position = turret.transform.position;
    }
}
