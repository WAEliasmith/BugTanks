using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTank : MonoBehaviour
{
    public float speed;
    private float backwardsMultiplier = 0.75f;
    public float xAxis;
    public float yAxis;
    public float rotationSpeed;

    public BoxCollider2D box;

    public Rigidbody2D rb;

    public float numAngles;

    public float rotDelay;
    public float rotStationaryUntilBurst;

    private float innerAngle;

    private float rotStationary;
    private float rotDelayLeft;
    public bool dead = false;
    public Transform killedBy;
    void Start()
    {
        killedBy = null;
    }
    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        if (!dead)
        {
            float rotation = Mathf.Clamp(xAxis, -1, 1) * rotationSpeed;
            rotDelayLeft--;
            if (rotDelayLeft < 0)
            {
                if (rotStationary > rotStationaryUntilBurst && xAxis != 0)
                {
                    //Tap rotation:
                    rotStationary = 0;
                    rotDelayLeft = rotDelay;
                    //Find next rotation amount
                    innerAngle = rb.rotation;
                    innerAngle += Mathf.Clamp(xAxis, -1, 1) * 360 / numAngles;
                    rotation = Mathf.Round(innerAngle / (360 / numAngles)) * (360 / numAngles);
                    rb.rotation = rotation;
                }
                else
                {
                    innerAngle += rotation;
                    rotation = Mathf.Round(innerAngle / (360 / numAngles)) * (360 / numAngles);
                    rb.rotation = rotation;
                }
            }
            float moveAmount = 0f;

            if (yAxis > 0.2f)
            {
                moveAmount = speed;
            }
            else if (yAxis < -0.2f)
            {
                moveAmount = -speed * backwardsMultiplier;
            }
            Vector2 displacementThisFrame = new Vector2(moveAmount * Mathf.Cos(Mathf.Deg2Rad * rb.rotation), moveAmount * Mathf.Sin(Mathf.Deg2Rad * rb.rotation));

            rb.MovePosition(rb.position + displacementThisFrame);

            if ((rb.rotation % 90 < 1 && rb.rotation % 90 > -1) || rb.rotation % 90 > 89 || rb.rotation % 90 < -89)
            {
                gameObject.layer = 7;
            }
            else
            {
                gameObject.layer = 6;
            }

            if (xAxis == 0)
            {
                rotStationary++;
            }
            else
            {
                rotStationary = 0;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
