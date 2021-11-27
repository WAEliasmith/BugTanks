using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MoveTank movement;
    public Gun gun;
    public hurtbox hbox;

    // public MazeHandler maze;

    // Update is called once per frame
    void Update()
    {
        movement.xAxis = -Input.GetAxis("Horizontal");
        movement.yAxis = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Fire") && movement.dead == false)
        {
            gun.Shoot();
        }
        // if (Input.GetKeyDown("1"))
        // {
        //     maze.changeWall(transform.position, "XW");
        // }
        // if (Input.GetKeyDown("2"))
        // {
        //     maze.changeWall(transform.position, "YW");
        // }
        // if (Input.GetKeyDown("3"))
        // {
        //     maze.changeWall(transform.position, "TXW");
        // }
        // if (Input.GetKeyDown("4"))
        // {
        //     maze.changeWall(transform.position, "TYW");
        // }
        // if (Input.GetKeyDown("5"))
        // {
        //     maze.changeWall(transform.position);
        // }

        if (hbox.hp <= 0)
        {
            movement.dead = true;
        }
    }
}
