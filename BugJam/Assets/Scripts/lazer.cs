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
                    if (travelDistThisFrame > ((Vector2)path[0] - (Vector2)transform.position).magnitude)
                    {
                        //go to next node
                        travelDistThisFrame -= ((Vector2)path[0] - (Vector2)transform.position).magnitude;
                        RaycastHit2D hit = Physics2D.Raycast(
                            transform.position, ((Vector2)path[0] - (Vector2)transform.position), ((Vector2)path[0] - (Vector2)transform.position).magnitude, layerMask);
                        if (hit.collider != null && hit.collider.tag == "Player")
                        {
                            hit.collider.GetComponent<hurtbox>().IHaveBeenHit(new Vector2(0, 0));
                            //add score to the player that owns me
                            GameObject.Find("ScoreHandler").GetComponent<ScoreHandler>()
                            .AddScore(ownerScoreNumber);

                            dead = true;
                            path[0] = (Vector2)transform.position + ((Vector2)path[0] - (Vector2)transform.position).normalized * hit.distance;
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
                        if (hit.collider != null && hit.collider.tag == "Player")
                        {
                            hit.collider.GetComponent<hurtbox>().IHaveBeenHit(new Vector2(0, 0));
                            dead = true;
                            goal = transform.position + (goal - transform.position).normalized * hit.distance;
                            tr.AddPosition(goal);
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
