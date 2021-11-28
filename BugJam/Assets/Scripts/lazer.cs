using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazer : MonoBehaviour
{
    public TrailRenderer tr;
    public float range = 10f;
    public List<Vector3> path = new List<Vector3>();

    public Vector2 direction;
    public float speed = 2f;
    public float deadTime = 10f;
    public bool dead;
    public int ownerScoreNumber;

    IEnumerator ResetTrailRenderer(TrailRenderer tr)
    {
        float trailTime = tr.time;
        tr.time = 0;
        yield return null;
        tr.time = trailTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = 1 << 6 | 1 << 7;

        if (!dead)
        {
            //follow path
            if (path.Count > 0)
            {
                //go towards next node
                direction = ((Vector2)path[0] - (Vector2)transform.position).normalized;
                var travelDistThisFrame = speed;
                for (int i = 0; i < 25; i++)
                {
                    if (path[0].z == -999f)
                    {
                        GameObject child = transform.GetChild(0).gameObject;
                        tr.AddPosition(transform.position);
                        if (child != null)
                        {
                            GameObject child2 = Instantiate(child);
                            child2.transform.parent = gameObject.transform;
                            child.transform.parent = null;
                            child2.transform.localPosition = new Vector3(0, 0, 0);

                            tr = child2.GetComponent<TrailRenderer>();
                        }
                        //next three nodes should be skipped
                        //transform.position = new Vector3(path[0].x, path[0].y, 120f);
                        path.RemoveAt(0);
                        //transform.position = new Vector3(path[0].x, path[0].y, 120f);
                        path.RemoveAt(0);
                        //transform.position = new Vector3(path[0].x, path[0].y, -1f);
                        transform.position = path[0];
                        path.RemoveAt(0);
                        tr.AddPosition(transform.position);

                    }

                    //go to next node

                    if (travelDistThisFrame > ((Vector2)path[0] - (Vector2)transform.position).magnitude)
                    {
                        travelDistThisFrame -= ((Vector2)path[0] - (Vector2)transform.position).magnitude;
                        RaycastHit2D hit = Physics2D.Raycast(
                            transform.position, ((Vector2)path[0] - (Vector2)transform.position), ((Vector2)path[0] - (Vector2)transform.position).magnitude, layerMask);
                        if (hit.collider != null && (hit.collider.tag == "Player" || hit.collider.tag == "Enemy"))
                        {
                            if (hit.collider.GetComponent<hurtbox>().iFrames <= 0)
                            {
                                hit.collider.GetComponent<hurtbox>().IHaveBeenHit(new Vector2(0, 0));

                                //add score to the player that owns me if not self hit
                                if (hit.collider.GetComponent<hurtbox>().gun.scoreNumber != ownerScoreNumber)
                                {
                                    settingsHandler.instance.AddScore(ownerScoreNumber);
                                }

                                dead = true;
                                i = 999;
                                path[0] = (Vector2)transform.position + ((Vector2)path[0] - (Vector2)transform.position).normalized * hit.distance;

                            }
                        }
                        transform.position = (Vector2)path[0];
                        tr.AddPosition(transform.position);

                        path.RemoveAt(0);
                        if (path.Count > 0)
                        {
                            direction = ((Vector2)path[0] - (Vector2)transform.position).normalized;
                        }
                        else
                        {
                            //reached end of path
                            i = 999;
                            dead = true;
                        }
                    }
                    else
                    {
                        //go towards end of path
                        Vector3 goal = transform.position + ((Vector3)direction) * travelDistThisFrame;
                        RaycastHit2D hit = Physics2D.Raycast(
                            transform.position, (goal - transform.position), (goal - transform.position).magnitude, layerMask);
                        if (hit.collider != null && (hit.collider.tag == "Player" || hit.collider.tag == "Enemy"))
                        {
                            if (hit.collider.GetComponent<hurtbox>().iFrames <= 0)
                            {
                                hit.collider.GetComponent<hurtbox>().IHaveBeenHit(new Vector2(0, 0));

                                //add score to the player that owns me if not self hit
                                if (hit.collider.GetComponent<hurtbox>().gun.scoreNumber != ownerScoreNumber)
                                {
                                    settingsHandler.instance.AddScore(ownerScoreNumber);
                                }

                                dead = true;
                                i = 999;
                                goal = transform.position + (goal - transform.position).normalized * hit.distance;
                                tr.AddPosition(goal);
                            }
                        }
                        transform.position = goal;
                    }
                }
            }

        }
        else
        {
            //bullet is dead
            deadTime--;
            if (deadTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
