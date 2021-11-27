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

    public bool wings = false;

    public Rigidbody2D rb;

    public float numAngles;

    public float rotDelay;
    public float rotStationaryUntilBurst;

    public float innerAngle;

    private float rotStationary;
    private float rotDelayLeft;
    public bool dead = false;
    public Transform killedBy = null;

    public float launchVectorDecay = 0.05f;
    public Vector2 launchVector;
    public float launchStrength = 1f;

    public void recoil(Vector2 recoil)
    {
        launchVector += recoil;
    }

    void Awake()
    {
        launchVector = new Vector2(0, 0);
        wings = false;
        rb.rotation = innerAngle;
        float rotation = Mathf.Round(innerAngle / (360 / numAngles)) * (360 / numAngles);
        rb.rotation = rotation;
    }
    // FixedUpdate is called once per physics frame

    void FixedUpdate()
    {
        launchVector -= launchVector * launchVectorDecay;

        Vector2 displacementThisFrame = new Vector2(0, 0);

        if (!dead)
        {
            float rotation = 0f;
            float moveAmount = 0f;
            if (wings == false)
            {
                //wingless movement
                rotation = Mathf.Clamp(xAxis, -1, 1) * rotationSpeed;
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
                if (yAxis > 0.2f)
                {
                    moveAmount = speed;
                }
                else if (yAxis < -0.2f)
                {
                    moveAmount = -speed * backwardsMultiplier;
                }
            }
            else
            {

                //winged movement
                if (xAxis != 0 || yAxis != 0)
                {
                    moveAmount = speed;
                    innerAngle = rb.rotation;
                    Vector2 vector = new Vector2(-Mathf.Clamp(xAxis, -1, 1), Mathf.Clamp(yAxis, -1, 1)).normalized;
                    float targetAngle = Vector2.SignedAngle(new Vector2(1f, 0f), vector);

                    rotation = Mathf.Round(targetAngle / (360 / numAngles)) * (360 / numAngles);
                    rb.rotation = rotation;
                }
            }

            displacementThisFrame = new Vector2(moveAmount * Mathf.Cos(Mathf.Deg2Rad * rb.rotation), moveAmount * Mathf.Sin(Mathf.Deg2Rad * rb.rotation));

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
        rb.MovePosition(rb.position + displacementThisFrame + launchVector * launchStrength * Time.deltaTime);

    }
}
