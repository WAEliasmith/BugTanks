using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Missile : bullet
{
    public int trackStart = 320;
    private int currentWaypoint = 0;
    private float nextWaypointDistance = 0.1f;
    public float rotateSpeed;
    private Transform Target;
    public Path path;
    public Seeker seeker;
    private float xAxis;

    public bool playerOnly = false;


    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        lifeLeft = life;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void SpecificAction()
    {
        if (lifeLeft < trackStart)
        {
            Target = GetClosestHurtbox();
            seeker.StartPath(transform.position, Target.position, OnPathComplete);

            Track();
        }
        //Convert velocity to angle to
        float angle = Vector2.SignedAngle(new Vector2(1f, 0f), velocity);
        angle += xAxis * rotateSpeed;
        velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle),
        Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * speedOverride;
        transform.eulerAngles = new Vector3(0f, 0f, angle);

    }

    Transform GetClosestHurtbox()
    {
        Transform closestHurtboxTransform = null;
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Obj.name == "Hurtbox")
            {
                if (closestHurtboxTransform == null ||
                Vector2.Distance(transform.position, closestHurtboxTransform.position)
                > Vector2.Distance(transform.position, Obj.transform.position))
                {
                    //Do Something
                    closestHurtboxTransform = Obj.transform;
                }
            }
        }
        if (playerOnly == false)
        {
            foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Obj.name == "Hurtbox")
                {
                    if (closestHurtboxTransform == null ||
                    Vector2.Distance(transform.position, closestHurtboxTransform.position)
                    > Vector2.Distance(transform.position, Obj.transform.position))
                    {
                        //Do Something
                        closestHurtboxTransform = Obj.transform;
                    }
                }
            }
        }


        return (closestHurtboxTransform);
    }

    void Track()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reached end of path
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
            xAxis = 1;
        }
        else if (dangle < -10)
        {
            xAxis = -1;
        }

        if (increaseWaypoint)
        {
            currentWaypoint++;
        }
    }
}
