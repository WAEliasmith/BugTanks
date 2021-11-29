using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject muzzleFlash = null;

    public bool fire;
    public int bulletCount = 5;
    public GameObject[] myBullets;

    public GameObject bullet;
    public GameObject gunPos;
    public float shotSpeed;
    private int wallLayerMask = 1 << 8 | 1 << 9;
    private int lazerLayerMask = 1 << 8 | 1 << 9 | 1 << 11;

    public MoveTank movement;

    public string powerup = "none";

    public bool crisp = false;

    public hurtbox hbox = null;

    public GameObject weird = null;
    public GameObject lazer = null;
    public GameObject missile = null;
    public GameObject wifimissile = null;
    public GameObject shockwave = null;
    public GameObject fragShot = null;
    public GameObject rpg = null;
    public GameObject grenade = null;
    public GameObject absorbShot = null;
    public GameObject crispShot = null;
    public float wingDuration = 350;

    public Color tankColor;
    public Color baseColor;

    public int scoreNumber = -1;

    private int weirdCount = 0;
    private int absorbCount = 0;
    private int absorbShots = 3;
    private int weirdShots = 5;

    public float minShotDistance = 0.2f;
    public bool explodeFrag = false;
    private float wingTimer = 999;

    public float lazerRange = 10;

    public LineRenderer line;
    private int currentScore;

    private Vector2 screenSize;
    private Vector2 wrapOffset;

    public GameObject pointExplosion;

    void Awake()
    {
        if (settingsHandler.instance && settingsHandler.instance.crisp == true)
        {
            crisp = true;
        }
    }
    void Start()
    {
        baseColor = tankColor;
        currentScore = settingsHandler.instance.scores[scoreNumber];
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;
        wrapOffset = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().wrapOffset;
        myBullets = new GameObject[bulletCount];
    }

    void Update()
    {
        if (settingsHandler.instance.pvp)
        {
            if (currentScore != settingsHandler.instance.scores[scoreNumber])
            {
                //Just got a point
                GameObject p = Instantiate(pointExplosion, Vector3.zero, Quaternion.identity);
                p.transform.position = transform.position;
                currentScore = settingsHandler.instance.scores[scoreNumber];
            }
        }
        if (hbox && hbox.iFrames > 0 && hbox.iFrames < 999)
        {
            tankColor = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            tankColor = baseColor;
        }

        if (hbox && hbox.iFrames > 999)
        {
            if (Mathf.Round(hbox.iFrames) % 10 == 0)
            {
                GameObject p = Instantiate(pointExplosion, Vector3.zero, Quaternion.identity);
                p.transform.position = transform.position;
            }
        }

        if (wingTimer < 200)
        {
            tankColor.a = (wingTimer / 200) * 1f;// + 0.5f;
        }


        if (powerup == "lazer")
        {
            line.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f + gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);
            RaycastHit2D hit2 = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f - gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);

            if (hit.collider == null && hit2.collider == null)
            {
                List<Vector3> lazerArray = CalculatePath();
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

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public void FixedUpdate()
    {
        if (powerup == "wings")
        {
            if (wingTimer == 999)
            {
                movement.wings = true;
                wingTimer = wingDuration;
            }

            if (wingTimer <= 1)
            {
                tankColor.a = 1f;
                wingTimer = 999;
                powerup = "none";
                movement.wings = false;
            }
            else
            {
                wingTimer -= 1;

            }
        }

    }

    public void Flash()
    {
        GameObject p = Instantiate(muzzleFlash, gunPos.transform.position, Quaternion.identity);
        p.transform.position = gunPos.transform.position;
        float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), gunPos.transform.right);
        p.transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
    }

    public void Shoot()
    {
        if (powerup == "frag exploder")
        {
            powerup = "none";
            return;
        }
        else if (powerup == "shockwave")
        {
            movement.recoil(-0.1f * gunPos.transform.right);

            for (int i = 0; i < 3; i++)
            {
                GameObject p = Instantiate(shockwave, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<bullet>().velocity = Rotate((Vector2)gunPos.transform.right, (i - 1) * 8f) * shotSpeed;
                powerup = "none";
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
            }
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f + gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);
        RaycastHit2D hit2 = Physics2D.Raycast(gunPos.transform.position - gunPos.transform.right * 0.1f - gunPos.transform.up * 0.06f, gunPos.transform.right, minShotDistance, wallLayerMask);

        if (hit.collider == null && hit2.collider == null)
        {
            if (powerup == "none" || powerup == "wings")
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
                    GameObject p;
                    if (crisp)
                    {
                        p = Instantiate(crispShot, gunPos.transform.position, Quaternion.identity);

                    }
                    else
                    {
                        p = Instantiate(bullet, gunPos.transform.position, Quaternion.identity);
                    }
                    p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
                    myBullets[shotIndex] = p;
                    p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                    Flash();
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
                    p.GetComponent<lazer>().ownerScoreNumber = scoreNumber;
                    p.transform.position = lazerArray[0];
                }
            }
            else if (powerup == "missile")
            {
                GameObject p = Instantiate(missile, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<Missile>().velocity = gunPos.transform.right * shotSpeed;
                powerup = "none";
                p.GetComponent<Missile>().ownerScoreNumber = scoreNumber;
                Flash();
            }
            else if (powerup == "wifi missile")
            {
                movement.recoil(-0.1f * gunPos.transform.right);
                GameObject p = Instantiate(wifimissile, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<WifiMissile>().velocity = gunPos.transform.right * shotSpeed;
                p.GetComponent<WifiMissile>().movement = movement;
                powerup = "none";
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                Flash();
            }
            else if (powerup == "frag shot")
            {
                movement.recoil(-0.5f * gunPos.transform.right);
                GameObject p = Instantiate(fragShot, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<FragGrenade>().velocity = gunPos.transform.right * shotSpeed;
                powerup = "frag exploder";
                p.GetComponent<FragGrenade>().Owner = gameObject;
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                Flash();
            }
            else if (powerup == "rpg")
            {
                movement.recoil(-1f * gunPos.transform.right);
                GameObject p = Instantiate(rpg, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<rpg>().velocity = gunPos.transform.right * shotSpeed;
                powerup = "none";
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                Flash();
            }
            else if (powerup == "grenade")
            {
                movement.recoil(-0.2f * gunPos.transform.right);
                GameObject p = Instantiate(grenade, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<Grenade>().velocity = gunPos.transform.right * shotSpeed;
                powerup = "none";
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                Flash();
            }
            else if (powerup == "weird")
            {
                GameObject p = Instantiate(weird, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                weirdCount++;
                if (weirdCount % weirdShots == 0)
                {
                    powerup = "none";
                }
            }
            else if (powerup == "absorb shot")
            {
                movement.recoil(-0.1f * gunPos.transform.right);
                GameObject p = Instantiate(absorbShot, gunPos.transform.position, Quaternion.identity);
                p.GetComponent<bullet>().velocity = gunPos.transform.right * shotSpeed;
                p.GetComponent<bullet>().ownerScoreNumber = scoreNumber;
                absorbCount++;
                if (absorbCount % absorbShots == 0)
                {
                    powerup = "none";
                }
                Flash();
            }
        }
    }

    private List<Vector3> CalculatePath()
    {
        //Shoot ray
        RaycastHit2D hit;
        Vector3 initialShotPos = gunPos.transform.position;

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

        float maxDist = lazerRange;

        Vector2 shotPos = (Vector2)initialShotPos;

        Vector2 prepDirection = gunPos.transform.right;

        List<Vector3> currPath = new List<Vector3>();
        currPath.Add(initialShotPos);
        // RaycastHit2D testhit = Physics2D.Raycast(gunPos.transform.position - 
        //gunPos.transform.right * 0.1f, gunPos.transform.right, minShotDistance, lazerLayerMask);
        // if (testhit == null)
        // {
        //     return null;
        // }
        //calculate currPath
        for (int i = 0; i < 25; i++)
        {
            hit = Physics2D.Raycast(shotPos, prepDirection, maxDist, lazerLayerMask);
            // Does the ray intersect
            if (hit.collider != null)
            {

                //Debug.DrawRay(shotPos, prepDirection * hit.distance, Color.yellow);

                shotPos = hit.point;
                maxDist -= hit.distance;
                currPath.Add((Vector3)(shotPos));


                bool sus = false;
                if (shotPos.x > (screenSize.x / 2) + wrapOffset.x)
                {
                    sus = true;
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    shotPos -= new Vector2(screenSize.x, 0f);
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, 0f));
                }
                else if (shotPos.x < -(screenSize.x / 2) + wrapOffset.x)
                {
                    sus = true;
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    shotPos += new Vector2(screenSize.x, 0f);
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, 0f));
                }
                else if (shotPos.y > (screenSize.y / 2) + wrapOffset.y)
                {
                    sus = true;
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    shotPos -= new Vector2(0f, screenSize.y);
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, 0f));
                }
                else if (shotPos.y < -(screenSize.y / 2) + wrapOffset.y)
                {
                    sus = true;
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    shotPos += new Vector2(0f, screenSize.y);
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, -999f));
                    currPath.Add(new Vector3(shotPos.x, shotPos.y, 0f));
                }


                float x = hit.normal.x;
                float y = hit.normal.y;

                if (!sus)
                {
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
