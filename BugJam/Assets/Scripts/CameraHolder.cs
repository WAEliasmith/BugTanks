using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraHolder : MonoBehaviour
{
    public Transform target = null;
    public Transform target2 = null;
    public bool follow = true;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    public Vector2 screenSize;
    public Vector2 wrapOffset;

    public GameObject NW;
    public GameObject N;
    public GameObject NE;
    public GameObject W;
    public GameObject C;
    public GameObject E;
    public GameObject SW;
    public GameObject S;
    public GameObject SE;

    private int prep = 4;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

    }

    void Start()
    {
        //disable outer cameras
        // NW.GetComponent<Camera>().enabled = false;
        // N.GetComponent<Camera>().enabled = false;
        // NE.GetComponent<Camera>().enabled = false;
        // W.GetComponent<Camera>().enabled = false;
        // E.GetComponent<Camera>().enabled = false;
        // SW.GetComponent<Camera>().enabled = false;
        // S.GetComponent<Camera>().enabled = false;
        // SE.GetComponent<Camera>().enabled = false;

        float w = screenSize.x;
        float h = screenSize.y;

        NW.transform.localPosition = new Vector3(-w, h, 0);
        N.transform.localPosition = new Vector3(0, h, 0);
        NE.transform.localPosition = new Vector3(w, h, 0);
        W.transform.localPosition = new Vector3(-w, 0, 0);
        C.transform.localPosition = new Vector3(0, 0, 0);
        E.transform.localPosition = new Vector3(w, 0, 0);
        SW.transform.localPosition = new Vector3(-w, -h, 0);
        S.transform.localPosition = new Vector3(0, -h, 0);
        SE.transform.localPosition = new Vector3(w, -h, 0);
    }
    // Fixed is called once per physics
    void FixedUpdate()
    {
        if (prep > 0)
        {
            prep--;
            if (prep == 1)
            {
                target = GameObject.Find("PlayerTank(Clone)").transform;
            }
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("main");
        }
        if (prep == 0 && follow && target.GetComponent<MoveTank>().dead == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position,
            target.position + offset, ref velocity, smoothSpeed);
        }
        else
        {
            {
                transform.position = Vector3.SmoothDamp(transform.position,
                new Vector3(0, 0, 0) + offset, ref velocity, smoothSpeed);
            }
        }
    }
}
