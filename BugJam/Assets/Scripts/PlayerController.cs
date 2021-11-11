using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MoveTank movement;
    public Gun gun;

    // Update is called once per frame
    void Update()
    {
        movement.xAxis = -Input.GetAxis("Horizontal");
        movement.yAxis = Input.GetAxis("Vertical");
        if(Input.GetButtonDown("Fire")){
            gun.Shoot();
        }
    }
}
