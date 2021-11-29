using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MoveTank movement;
    public Gun gun;
    public hurtbox hbox;
    public int playerControlsNumber = 0;

    // public MazeHandler maze;

    // Update is called once per frame
    void Update()
    {
        if (movement.dead == false)
        {
            if (playerControlsNumber == 0)
            {
                movement.xAxis = -Input.GetAxis("Horizontal");
                movement.yAxis = Input.GetAxis("Vertical");
                if (Input.GetButtonDown("Fire"))
                {
                    gun.Shoot();
                }
            }
            else if (playerControlsNumber == 1)
            {
                movement.xAxis = -Input.GetAxis("Horizontal1");
                movement.yAxis = Input.GetAxis("Vertical1");
                if (Input.GetButtonDown("Fire1"))
                {
                    gun.Shoot();
                }
            }
            else if (playerControlsNumber == 2)
            {
                movement.xAxis = -Input.GetAxis("Horizontal2");
                movement.yAxis = Input.GetAxis("Vertical2");
                if (Input.GetButtonDown("Fire2"))
                {
                    gun.Shoot();
                }
            }
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
    }
}
