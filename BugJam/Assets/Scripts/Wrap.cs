using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour
{
    public Transform cameraTransform = null;

    private Vector2 screenSize;
    private Vector2 wrapOffset;

    public bool holdCamera = false;

    public bullet weirdBullet = null;

    void Start()
    {
        if (holdCamera)
        {
            cameraTransform = GameObject.Find("CameraHolder").transform;

        }
        screenSize = settingsHandler.instance.screenSize;
        wrapOffset = settingsHandler.instance.wrapOffset;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (cameraTransform != null
        && cameraTransform.GetComponent<CameraHolder>().follow)
        {
            if (transform.position.x > (screenSize.x / 2) + wrapOffset.x)
            {
                cameraTransform.position -= new Vector3(screenSize.x, 0f, 0f);
            }
            else if (transform.position.x < -(screenSize.x / 2) + wrapOffset.x)
            {
                cameraTransform.position += new Vector3(screenSize.x, 0f, 0f);
            }
            if (transform.position.y > (screenSize.y / 2) + wrapOffset.y)
            {
                cameraTransform.position -= new Vector3(0f, screenSize.y, 0f);
            }
            else if (transform.position.y < -(screenSize.y / 2) + wrapOffset.y)
            {
                cameraTransform.position += new Vector3(0f, screenSize.y, 0f);
            }
        }


        if (transform.position.x > (screenSize.x / 2) + wrapOffset.x)
        {
            transform.position -= new Vector3(screenSize.x, 0f, 0f);
            if (weirdBullet != null && weirdBullet.lifeLeft != weirdBullet.life - 1)
            {
                weirdBullet.noweirdnextframe = 2;
            }
        }
        else if (transform.position.x < -(screenSize.x / 2) + wrapOffset.x)
        {
            transform.position += new Vector3(screenSize.x, 0f, 0f);
            if (weirdBullet != null && weirdBullet.lifeLeft != weirdBullet.life - 1)
            {
                weirdBullet.noweirdnextframe = 2;
            }
        }
        if (transform.position.y > (screenSize.y / 2) + wrapOffset.y)
        {
            transform.position -= new Vector3(0f, screenSize.y, 0f);
            if (weirdBullet != null && weirdBullet.lifeLeft != weirdBullet.life - 1)
            {
                weirdBullet.noweirdnextframe = 2;
            }
        }
        else if (transform.position.y < -(screenSize.y / 2) + wrapOffset.y)
        {
            transform.position += new Vector3(0f, screenSize.y, 0f);
            if (weirdBullet != null && weirdBullet.lifeLeft != weirdBullet.life - 1)
            {
                weirdBullet.noweirdnextframe = 2;
            }
        }
    }
}
