using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrap : MonoBehaviour
{

    public GameObject CameraHolder = null;

    private Vector2 screenSize;
    private Vector2 wrapOffset;

    void Start()
    {
        screenSize = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().screenSize;
        wrapOffset = GameObject.Find("CameraHolder").GetComponent<CameraHolder>().wrapOffset;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x > (screenSize.x / 2) + wrapOffset.x)
        {
            transform.position -= new Vector3(screenSize.x, 0f, 0f);
            if (CameraHolder != null && CameraHolder.GetComponent<CameraHolder>().follow)
            {
                CameraHolder.transform.position -= new Vector3(screenSize.x, 0f, 0f);
            }
        }
        else if (transform.position.x < -(screenSize.x / 2) + wrapOffset.x)
        {
            transform.position += new Vector3(screenSize.x, 0f, 0f);
            if (CameraHolder != null && CameraHolder.GetComponent<CameraHolder>().follow)
            {
                CameraHolder.transform.position += new Vector3(screenSize.x, 0f, 0f);
            }
        }
        if (transform.position.y > (screenSize.y / 2) + wrapOffset.y)
        {
            transform.position -= new Vector3(0f, screenSize.y, 0f);
            if (CameraHolder != null && CameraHolder.GetComponent<CameraHolder>().follow)
            {
                CameraHolder.transform.position -= new Vector3(0f, screenSize.y, 0f);
            }
        }
        else if (transform.position.y < -(screenSize.y / 2) + wrapOffset.y)
        {
            transform.position += new Vector3(0f, screenSize.y, 0f);
            if (CameraHolder != null && CameraHolder.GetComponent<CameraHolder>().follow)
            {
                CameraHolder.transform.position += new Vector3(0f, screenSize.y, 0f);
            }
        }
    }
}
