using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiMissile : absorption
{
    public MoveTank movement;
    public float rotateSpeed;

    public override void SpecificAction()
    {

        //Convert velocity to angle to
        float angle = Vector2.SignedAngle(new Vector2(1f, 0f), velocity);
        if (movement != null)
        {
            angle += movement.xAxis * rotateSpeed;
        }
        velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle),
        Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * speedOverride;
        transform.eulerAngles = new Vector3(0f, 0f, angle);

    }

}
