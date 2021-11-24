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
    private int wallLayerMask = 1 << 8 | 1 << 9;
    public MoveTank movement;

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

    public float minShotDistance = 0.2f;
    public bool explodeFrag = false;

    public float lazerRange = 10;

    public LineRenderer line;

    private Vector2 screenSize;
    private Vector2 wrapOffset;

    void Start()
    {
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;
        wrapOffset = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().wrapOffset;
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
        if (powerup == "frag exploder")
        {
            powerup = "none";
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f + gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f - gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);

        if (hit.collider == null && hit2.collider == null)
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
                    p.transform.position = lazerArray[0];
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
            else if (powerup == "frag shot")
            {
                movement.recoil(-0.5f * gunPos.transform.right);
                GameObject p = Instantiate(fragShot, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<FragGrenade>().velocity = gunPos.transform.right * shotSpeed;
                p.GetComponent<FragGrenade>().Owner = gameObject;
                powerup = "frag exploder";
            }
            else if (powerup == "rpg")
            {
                movement.recoil(-1f * gunPos.transform.right);
                GameObject p = Instantiate(rpg, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<rpg>().velocity = gunPos.transform.right * shotSpeed;
                powerup = "none";
            }
            else if (powerup == "grenade")
            {
                movement.recoil(-0.2f * gunPos.transform.right);
                GameObject p = Instantiate(grenade, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<Grenade>().velocity = gunPos.transform.right * shotSpeed;
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
    }

    private List<Vector3> CalculatePath()
    {
        //Shoot ray
        RaycastHit2D hit;
        Vector3 initialShotPos = transform.position;

        Debug.Log("initialShotPos" + initialShotPos);

        if (initialShotPos.x > (screenSize.x / 2) + wrapOffset.x)
        {
            initialShotPos -= new Vector3(screenSize.x, 0f, 0f);
        }
        else if (initialShotPos.x < -(screenSize.x / 2) + wrapOffset.x)
        {
            initialShotPos += new Vector3(screenSize.x, 0f, 0f);
        }
        if (initialShotPos.y > (screenSize.y / 2) + wrapOffset.y)
        {
            initialShotPos -= new Vector3(0f, screenSize.y, 0f);
        }
        else if (initialShotPos.y < -(screenSize.y / 2) + wrapOffset.y)
        {
            initialShotPos += new Vector3(0f, screenSize.y, 0f);
        }

        RaycastHit2D testhit = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f, gunPos.transform.right, minShotDistance, wallLayerMask);
        if (testhit == null)
        {
            return null;
        }

        float maxDist = lazerRange;

        Vector2 shotPos = (Vector2)initialShotPos;

        Vector2 prepDirection = gunPos.transform.right;

        List<Vector3> currPath = new List<Vector3>();
        currPath.Add(initialShotPos);

        //calculate currPath
        for (int i = 0; i < 25; i++)
        {
            hit = Physics2D.Raycast(shotPos, prepDirection, maxDist, wallLayerMask);
            // Does the ray intersect
            if (hit.collider != null)
            {
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
