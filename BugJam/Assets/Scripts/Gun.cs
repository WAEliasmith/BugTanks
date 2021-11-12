using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool fire;
    public int bulletCount = 5;
    public GameObject[] myBullets;

    public GameObject bullet;
    public GameObject gunPos;
    public float shotSpeed;

    void Start()
    {
        myBullets = new GameObject[bulletCount];
    }

    public void Shoot()
    {
        int shotIndex = -1;
        for (int i = 0; i < myBullets.Length; i++)
        {
            Debug.Log(myBullets[i]);
            if (myBullets[i] == null)
            {
                shotIndex = i;
            }
        }
        if (shotIndex != -1)
        {
            GameObject p = Instantiate(bullet, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            myBullets[shotIndex] = p;
        }
    }
}
