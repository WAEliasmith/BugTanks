using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

public class wrapChild : MonoBehaviour
{
    public GameObject child;
    private GameObject[] clones;

    public Vector2 screenSize;

    void Awake()
    {
        clones = new GameObject[8];
        child = transform.GetChild(0).gameObject;
        for (int i = 0; i < 8; i++)
        {
            clones[i] = Instantiate(child);
            clones[i].transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector2 position = child.transform.position;
            if (i == 0 || i == 1 || i == 2)
            {
                position.y -= screenSize.y;
            }
            if (i == 5 || i == 6 || i == 7)
            {
                position.y += screenSize.y;
            }
            if (i == 0 || i == 3 || i == 5)
            {
                position.x -= screenSize.x;
            }
            if (i == 2 || i == 4 || i == 7)
            {
                position.x += screenSize.x;
            }
            clones[i].transform.position = position;
        }
    }
}

