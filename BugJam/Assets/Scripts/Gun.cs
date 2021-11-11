using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool fire;

    public GameObject bullet;
    public GameObject gunPos;
    public float shotSpeed;

    public void Shoot()
    {
        GameObject p = Instantiate(bullet, gunPos.transform.position, Quaternion.identity);
        p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
    }
}
