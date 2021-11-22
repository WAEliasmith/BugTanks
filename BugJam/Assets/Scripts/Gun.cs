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

    public string powerup = "none";

    public GameObject weird = null;
    public GameObject lazer = null;
    public GameObject missile = null;
    public GameObject wifimissile = null;
    public GameObject shockwave = null;
    public GameObject fragShot = null;
    public GameObject rpg = null;
    public GameObject grenade = null;
    private int weirdCount = 0;

    public float lazerRange = 10;

    public LineRenderer line;

    void Start()
    {
        myBullets = new GameObject[bulletCount];
    }

    void Update()
    {
        if (powerup == "lazer")
        {
            line.enabled = true;
            List<Vector3> lazerArray = CalculatePath();
            if (lazerArray != null)
            {
                line.positionCount = lazerArray.Count;
                line.SetPositions(lazerArray.ToArray());
            }
            else
            {
                line.enabled = false;
            }
        }
        else
        {
            line.enabled = false;
        }
    }

    public void Shoot()
    {
        if (powerup == "none")
        {
            int shotIndex = -1;
            for (int i = 0; i < myBullets.Length; i++)
            {
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
        else if (powerup == "lazer")
        {
            List<Vector3> lazerArray = CalculatePath();
            if (lazerArray != null)
            {
                GameObject p = Instantiate(lazer, gunPos.transform.position, Quaternion.identity);
                powerup = "none";
                p.GetComponent<lazer>().path = lazerArray;
            }
        }
        else if (powerup == "missile")
        {
            GameObject p = Instantiate(missile, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "none";
        }
        else if (powerup == "wifi missile")
        {
            GameObject p = Instantiate(wifimissile, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "none";
        }
        else if (powerup == "shockwave")
        {
            GameObject p = Instantiate(shockwave, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "none";
        }
        else if (powerup == "frag exploder")
        {
            powerup = "none";
        }
        else if (powerup == "frag shot")
        {
            GameObject p = Instantiate(fragShot, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "frag exploder";
        }
        else if (powerup == "rpg")
        {
            GameObject p = Instantiate(lazer, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "none";
        }
        else if (powerup == "grenade")
        {
            GameObject p = Instantiate(grenade, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            powerup = "none";
        }
        else if (powerup == "weird")
        {
            GameObject p = Instantiate(weird, gunPos.transform.position, Quaternion.identity);
            p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
            weirdCount++;
            if (weirdCount % 5 == 0)
            {
                powerup = "none";
            }
        }
    }

    private List<Vector3> CalculatePath()
    {
        //Shoot ray
        RaycastHit2D hit;
        int wallLayerMask = 1 << 8 | 1 << 9;
        Vector2 shotPos = (Vector2)transform.position;
        float maxDist = lazerRange;

        Vector2 prepDirection = gunPos.transform.right;

        List<Vector3> currPath = new List<Vector3>();
        currPath.Add(transform.position);

        //calculate currPath
        for (int i = 0; i < 25; i++)
        {
            hit = Physics2D.Raycast(shotPos, prepDirection, maxDist, wallLayerMask);
            // Does the ray intersect
            if (hit.collider != null)
            {
                if (i == 0)
                {
                    if (hit.distance < 0.2f)
                    {
                        return null;
                    }
                }
                currPath.Add((Vector3)(shotPos + prepDirection * hit.distance));
                Debug.DrawRay(shotPos, prepDirection * hit.distance, Color.yellow);

                shotPos = hit.point;
                maxDist -= hit.distance;

                float x = hit.normal.x;
                float y = hit.normal.y;

                //Make lazer bounce
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    prepDirection.x = -prepDirection.x;
                }
                else
                {
                    prepDirection.y = -prepDirection.y;
                }

                //Move new lazer point forwards a lil
                shotPos += prepDirection * 0.015f;
            }
            else
            {
                //and end pos

                currPath.Add((Vector3)(shotPos + prepDirection * maxDist));
                i = 999;
            }
        }

        return (currPath);
    }
}
