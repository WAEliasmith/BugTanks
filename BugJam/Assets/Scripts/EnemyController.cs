using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public MoveTank movement;
    public Gun gun;

    public Transform gunPos;

    public Transform Target;

    public float reload = 20;

    public float bulletCheckDist = 3;
    public float bulletCheckAngle = 80;

    private float reloadLeft;

    Path path;
    int currentWaypoint = 0;
    public float nextWaypointDistance = 0.3f;
    public int time = 30;
    private int timeLeft;
    public int dodgeCheckTime = 30;
    private int dodgeCheckTimeLeft;
    public int passiveShotCheckTime = 5;
    private int passiveShotCheckTimeLeft;
    public float AngrySinSpeed = 0.05f;
    public int angryTime = 60;
    public int trackTime = 60;
    public int chillTime = 60;
    public int chillRadius = 3;
    public float angryAngle = 45;

    public float playerDesire = 70f;
    public float anger = 30f;

    public float bulletSafeDist = 1f;
    Seeker seeker;

    private string state;

    void Start()
    {
        seeker = GetComponent<Seeker>();

        seeker.StartPath(transform.position, Target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, Target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    Collider2D CheckForBullets(bool safe = false)
    {
        Collider2D closestBullet = null;
        int layerMask = 1 << 3;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, bulletCheckDist, layerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //Check bullet direction vs my direction
            Vector2 bvel = hitColliders[i].GetComponent<bullet>().velocity;
            Vector2 bdirtome = transform.position - hitColliders[i].transform.position;

            float dangle = Mathf.DeltaAngle(Vector2.SignedAngle(new Vector2(1f, 0f), bdirtome), Vector2.SignedAngle(new Vector2(1f, 0f), bvel));
            if (Mathf.Abs(dangle) < bulletCheckAngle || (safe && (hitColliders[i].transform.position - transform.position).magnitude < bulletSafeDist))
            {
                //see bullet
                //Debug.DrawRay(hitColliders[i].transform.position, bvel * 4f, Color.yellow);
                if (closestBullet == null || (hitColliders[i].transform.position - transform.position).magnitude < (closestBullet.transform.position - transform.position).magnitude)
                {
                    closestBullet = hitColliders[i];
                }
            }
        }
        return (closestBullet);
    }

    // Update is called once per physics
    void FixedUpdate()
    {
        //Debug.Log(state);
        movement.xAxis = 0;
        movement.yAxis = 0;
        passiveShotCheckTimeLeft--;
        if (passiveShotCheckTimeLeft <= 0)
        {
            passiveShotCheckTimeLeft = passiveShotCheckTime;
            ShotCheck(true);
        }

        dodgeCheckTimeLeft--;
        if (dodgeCheckTimeLeft <= 0)
        {
            dodgeCheckTimeLeft = dodgeCheckTime;
            //check to see if I need to dodge
            Collider2D bullet = CheckForBullets();
            if (bullet != null && (bullet.transform.position - transform.position).magnitude < bulletCheckDist)
            {
                //Initiate dodge
                state = "dodge";
                timeLeft = time;
            }
        }
        timeLeft--;
        if (timeLeft <= 0)
        {
            state = GetNewState();
        }

        reloadLeft--;

        if (state == "track")
        {
            Track();
        }
        else if (state == "angry")
        {
            Angry();
        }
        else if (state == "dodge")
        {
            dodgeCheckTimeLeft++;
            timeLeft++;
            Dodge();
        }
    }

    void ShotCheck(bool passive)
    {
        //Shoot ray
        float maxDist = 10;
        if (passive) { maxDist -= 2; }
        Vector2 shotPos = (Vector2)gunPos.position;

        RaycastHit2D hit;

        int layerMask = 1 << 8 | 1 << 9 | 1 << 6 | 1 << 7 | 1 << 10;

        Vector2 direction = gunPos.right;

        //Debug.DrawRay(shotPos, direction, Color.green);

        for (int i = 0; i < 25; i++)
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
                if (Mathf.Abs(x) > Mathf.Abs(y) + 0.2)
                {
                    direction.x = -direction.x;
                }
                else if (Mathf.Abs(x) < Mathf.Abs(y) - 0.2)
                {
                    direction.y = -direction.y;
                }
                else
                {
                    direction.y = -direction.y;
                    direction.x = -direction.x;
                }

                //Move new lazer point forwards a lil
                shotPos += direction * 0.01f;
            }
            else
            {
                i = 999;
            }
        }

    }

    string GetNewState()
    {
        float r1 = Random.value * 100;

        if (r1 < anger)
        {
            timeLeft = angryTime;
            return ("angry");
        }

        r1 = Random.value * 100;

        if (r1 < playerDesire)
        {
            Target = GetClosestPlayer();
            timeLeft = trackTime;
            seeker.StartPath(transform.position, Target.position, OnPathComplete);
            return ("track");
        }

        timeLeft = chillTime;
        seeker.StartPath(transform.position, new Vector2(Random.value * chillRadius, Random.value * chillRadius), OnPathComplete);
        return ("track");
    }

    void Angry()
    {
        Vector2 direction = (Vector2)(Target.position - transform.position).normalized;
        float dangle = Mathf.DeltaAngle(transform.eulerAngles.z, Vector2.SignedAngle(new Vector2(1f, 0f), direction));
        dangle += angryAngle * (Mathf.Sin((timeLeft - angryTime) * AngrySinSpeed));
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

    void Track()
    {
        if (path == null)
        {
            float r1 = Random.value * 100;

            if (r1 < 80)
            {
                GetNewState();
            }
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reached end of path
            float r1 = Random.value * 100;

            if (r1 < 80)
            {
                GetNewState();
            }
            return;
        }
        bool increaseWaypoint = false;
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            increaseWaypoint = true;
        }

        Vector2 direction = (Vector2)(path.vectorPath[currentWaypoint] - transform.position).normalized;
        float dangle = Mathf.DeltaAngle(transform.eulerAngles.z, Vector2.SignedAngle(new Vector2(1f, 0f), direction));
        if (increaseWaypoint)
        {
            //avoid nervous twitch
            dangle = 0;
        }
        if (dangle > 10)
        {
            movement.xAxis = 1;
        }
        else if (dangle < -10)
        {
            movement.xAxis = -1;
        }


        if (Mathf.Abs(dangle) < 30)
        {
            movement.yAxis = 1;
        }

        if (increaseWaypoint)
        {
            currentWaypoint++;
        }
    }

    Transform GetClosestPlayer()
    {
        Transform closestHurtboxTransform = null;
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Obj.name == "Hurtbox")
            {
                if (closestHurtboxTransform == null ||
                Vector2.Distance(transform.position, closestHurtboxTransform.position)
                < Vector2.Distance(transform.position, Obj.transform.position))
                {
                    //Do Something
                    closestHurtboxTransform = Obj.transform;
                }
            }
        }

        return (closestHurtboxTransform);
    }

    void Dodge()
    {
        Collider2D closestBullet = CheckForBullets(true);
        if (closestBullet)
        {
            Vector2 bulletVel = closestBullet.GetComponent<bullet>().velocity;
            Vector2 perp = Vector3.Cross(bulletVel, new Vector3(0, 0, 1)).normalized;

            if (Vector2.Distance(transform.position, closestBullet.transform.position)
            < Vector2.Distance(transform.position, (Vector2)closestBullet.transform.position + perp * 0.01f))
            {
                perp = perp * -1f;
            }

            Debug.DrawRay(closestBullet.transform.position, perp, Color.red, 1f);

            //figure out whether forwards or backwards goes more perp away
            float dangle = Mathf.DeltaAngle(transform.eulerAngles.z, Vector2.SignedAngle(new Vector2(1f, 0f), perp));
            if (Mathf.Abs(dangle) < 100)
            {
                //go forwards
                movement.yAxis = 1;
            }
            else
            {
                //go backwards
                movement.yAxis = -1;
            }

            movement.xAxis = 0;
            //Rotate end I am moving towards perp
            if (dangle > 30 && dangle < 150)
            {
                movement.xAxis = 1 * movement.yAxis;
            }
            else if (dangle < -30 && dangle > -150)
            {
                movement.xAxis = -1 * movement.yAxis;
            }

        }
        else
        {
            //go backwards
            movement.yAxis = 0;
            timeLeft -= 10;
        }
    }
}