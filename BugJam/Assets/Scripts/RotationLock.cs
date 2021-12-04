using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public PlayerController player;
    public Rigidbody2D rb;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "YWall" || other.gameObject.tag == "XWall")
        {
            if ((rb.rotation % 90 < 1 && rb.rotation % 90 > -1) || rb.rotation % 90 > 89 || rb.rotation % 90 < -89)
            {
                player.turnLock = true;
                StartCoroutine(delay());
            }
        }
    }

    IEnumerator delay(float delayTime = 0.07f)
    {
        yield return new WaitForSeconds(delayTime);
        player.turnLock = false;
    }
}
