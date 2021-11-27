using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public float maxHp = 3;

    public float hp;

    public Gun gun;

    public MoveTank move;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    public void IHaveBeenHit(Vector2 enemyPos, float hitStrength = 0)
    {
        //tell the hurtbox they have been hit
        hp -= 1;
        move.launchVector += hitStrength * ((Vector2)transform.position - enemyPos).normalized;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            if (other.GetComponent<bullet>().dead == false)
            {
                IHaveBeenHit(other.transform.position, other.GetComponent<bullet>().strength);
                other.GetComponent<bullet>().hit();
                //add score to the player that hit me
                if (other.GetComponent<bullet>().ownerScoreNumber != gun.scoreNumber)
                {
                    //not self hitting
                    ScoreHandler.instance.AddScore(other.GetComponent<bullet>().ownerScoreNumber);
                }

            }
        }

        if (other.tag == "Powerup")
        {
            if (gun.powerup == "none")
            {
                gun.powerup = other.GetComponent<Powerup>().powerup;
                other.GetComponent<Powerup>().Collected();
            }
        }
    }
}
