using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public bool mad = false;
    public Gun gun;
    public Transform gunPos;
    public float reload = 20;
    public int time = 30;
    public int WallBounceAim = 25;
    public float bulletSafeDist = 1f;
    public int shotCheckTime = 5;
    private int shotCheckTimeLeft;
    private float reactionTime = 25f;
    private float timeLeft;
    private float reloadLeft;
    public float anger = 70;
    public float AngrySinSpeed = 0.2f;
    public float dontShootAtWallDist = 0.5f;
    public float aimDist = 10f;
    public float angryAngle = 60;
    public MoveTank movement = null;
    private int aimTime;



    private Transform Target;

    public string state = "chill";

    void Start()
    {
        if (gun.crisp)
        {
            WallBounceAim = 2;
        }
        reactionTime += Random.Range(0, 10f);
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        aimTime++;
        if (reactionTime > 0)
        {
            reactionTime--;
        }
        else
        {
            if (!movement.dead)
            {
                timeLeft--;
                if (timeLeft <= 0)
                {
                    state = GetNewState();
                }

                movement.xAxis = 0;
                movement.yAxis = 0;
                reloadLeft--;
                shotCheckTimeLeft--;
                if (shotCheckTimeLeft <= 0)
                {
                    shotCheckTimeLeft = shotCheckTime;
                    ShotCheck(false);
                }
                Angry();
            }
        }
    }

    void Angry()
    {
        Vector3 pos = transform.position;
        if (gun.fragOut != null)
        {
            pos = gun.fragOut.position;
        }
        Vector2 direction;
        if (Target)
        {
            direction = (Vector2)(Target.position - pos).normalized;
        }
        else
        {
            direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        }
        float dangle = Mathf.DeltaAngle(transform.eulerAngles.z, Vector2.SignedAngle(new Vector2(1f, 0f), direction));
        dangle += angryAngle * (Mathf.Sin(aimTime * AngrySinSpeed));
        if (dangle > 10)
        {
            movement.xAxis = 1f;
        }
        else if (dangle < -10)
        {
            movement.xAxis = -1f;
        }
        ShotCheck(false);
    }

    string GetNewState()
    {
        //need to be really cheeseAngry to cheese
        float r1 = Random.value * 100;

        if (r1 < anger)
        {
            timeLeft = timeLeft;
            Target = GetClosestPlayer();
            return ("angry");
        }

        timeLeft = time;
        return ("chill");
    }

    void ShotCheck(bool passive, bool fragPos = false)
    {
        if (gun.fragOut == null || (gun.fragOut.position - transform.position).magnitude > 1.5f)
        {
            if (!fragPos && gun.fragOut != null)
            {
                ShotCheck(false, true);
                passive = false;
            }
            //Shoot ray
            float maxDist = aimDist;
            if (passive) { maxDist -= 2; }
            Vector2 shotPos = (Vector2)gunPos.position;

            if (fragPos)
            {
                shotPos = gun.fragOut.position;
            }
            RaycastHit2D hit;

            int layerMask = 1 << 8 | 1 << 9 | 1 << 6 | 1 << 7;
            int wallLayerMask = 1 << 8 | 1 << 9;

            Vector2 direction = gunPos.right;

            Debug.DrawRay(shotPos, direction, Color.green);
            // subtract a small amount to avoid attempting to shoot over walls
            hit = Physics2D.Raycast(shotPos - (direction.normalized * 0.1f), direction, maxDist, wallLayerMask);
            if (hit.distance < dontShootAtWallDist)
            {
                //allow AI to pointblank player
                if (hit.collider != null && hit.collider.tag != "Player")
                {
                    return;
                }
            }

            float tempWallBounce = WallBounceAim;
            if (fragPos)
            {
                tempWallBounce = 1;
            }
            for (int i = 0; i < tempWallBounce; i++)
            {
                hit = Physics2D.Raycast(shotPos, direction, maxDist, layerMask);
                // Does the ray intersect
                if (hit.collider != null)
                {
                    float selfDist = 0.2f;
                    if (passive) { selfDist += 0.6f; }
                    if (hit.collider.tag == "Player")
                    {
                        i = 999;
                        if ((hit.collider.transform.position - transform.position).magnitude > selfDist)
                        {
                            if (reloadLeft <= 0)
                            {
                                //passive shot
                                gun.Shoot();
                                reloadLeft = reload;
                                if (passive) { reloadLeft += reload; }
                            }

                        }
                    }
                    Debug.DrawRay(shotPos, direction * hit.distance, Color.yellow, Mathf.Min(reloadLeft, 2));

                    shotPos = hit.point;
                    maxDist -= hit.distance;

                    float x = hit.normal.x;
                    float y = hit.normal.y;

                    //Make lazer bounce
                    if (Mathf.Abs(x) > Mathf.Abs(y) + 0.05)
                    {
                        direction.x = -direction.x;
                    }
                    else if (Mathf.Abs(x) < Mathf.Abs(y) - 0.05)
                    {
                        direction.y = -direction.y;
                    }
                    else
                    {
                        direction.y = -direction.y;
                        direction.x = -direction.x;
                    }

                    //Move new lazer point forwards a lil
                    shotPos += direction * 0.015f;
                    layerMask = 1 << 8 | 1 << 9 | 1 << 6 | 1 << 7;// | 1 << 10

                }
                else
                {
                    i = 999;
                }
            }
            if (mad == true)
            {
                if (reloadLeft <= 0)
                {
                    shotPos = (Vector2)gunPos.position;

                    hit = Physics2D.Raycast(shotPos, direction, maxDist, layerMask);

                    float selfDist = 0.2f;
                    if (hit.collider != null && (hit.collider.transform.position - transform.position).magnitude > selfDist)
                    {
                        gun.Shoot();
                        reloadLeft = reload;
                        reloadLeft += reload * 0.5f;
                    }
                }
            }
        }
    }

    Transform GetClosestPlayer()
    {
        Transform closestHurtboxTransform = null;
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Obj.name == "Hurtbox")
            {
                if (Vector2.Distance(transform.position, Obj.transform.position) < 0.1f)
                {
                    //stop targeting yourself
                }
                else if (closestHurtboxTransform == null ||
                Vector2.Distance(transform.position, closestHurtboxTransform.position)
                > Vector2.Distance(transform.position, Obj.transform.position))
                {
                    //Do Something
                    closestHurtboxTransform = Obj.transform;
                }
            }
        }

        return (closestHurtboxTransform);
    }
}
