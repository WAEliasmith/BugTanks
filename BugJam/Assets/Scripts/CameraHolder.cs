using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraHolder : MonoBehaviour
{
    public Transform target0 = null;
    public Transform target = null;
    public Transform target2 = null;
    public bool follow = true;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    public Vector2 screenSize;
    public Vector2 wrapOffset;

    public Transform screenShakeTarget;

    public GameObject NW;
    public GameObject N;
    public GameObject NE;
    public GameObject W;
    public GameObject C;
    public GameObject E;
    public GameObject SW;
    public GameObject S;
    public GameObject SE;

    public Camera RenderCamera;

    private int prep = 2;

    public float boss2offset2 = 0;


    private float initialZoomTarget;
    public float zoom = 4;
    public float zoomTarget = 4;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = screenShakeTarget.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            screenShakeTarget.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        screenShakeTarget.transform.localPosition = originalPos;
    }

    void Update()
    {
        smoothSpeed = 0.075f;
        zoom = zoom * 0.95f + zoomTarget * 0.05f;
        C.GetComponent<Camera>().orthographicSize = zoom;
    }

    void Start()
    {
        settingsHandler.instance.screenSize = screenSize;
        settingsHandler.instance.wrapOffset = wrapOffset;
        settingsHandler.instance.camera = GetComponent<CameraHolder>();
        if (settingsHandler.instance.pvp == true)
        {
            follow = settingsHandler.instance.cameraFollow;

        }
        initialZoomTarget = zoomTarget;
        if (settingsHandler.instance.numPlayers == 1)
        {
            target = target0;
            target2 = null;
        }

        float w = screenSize.x;
        float h = screenSize.y;

        RenderCamera.orthographicSize = h * 0.5f;

        NW.transform.localScale = new Vector3(w, h, 1f);
        N.transform.localScale = new Vector3(w, h, 1f);
        NE.transform.localScale = new Vector3(w, h, 1f);
        W.transform.localScale = new Vector3(w, h, 1f);
        E.transform.localScale = new Vector3(w, h, 1f);
        SW.transform.localScale = new Vector3(w, h, 1f);
        S.transform.localScale = new Vector3(w, h, 1f);
        SE.transform.localScale = new Vector3(w, h, 1f);

        NW.transform.localPosition = new Vector3(-w, h, 1) + (Vector3)wrapOffset;
        N.transform.localPosition = new Vector3(0, h, 1) + (Vector3)wrapOffset;
        NE.transform.localPosition = new Vector3(w, h, 1) + (Vector3)wrapOffset;
        W.transform.localPosition = new Vector3(-w, 0, 1) + (Vector3)wrapOffset;
        E.transform.localPosition = new Vector3(w, 0, 1) + (Vector3)wrapOffset;
        SW.transform.localPosition = new Vector3(-w, -h, 1) + (Vector3)wrapOffset;
        S.transform.localPosition = new Vector3(0, -h, 1) + (Vector3)wrapOffset;
        SE.transform.localPosition = new Vector3(w, -h, 1) + (Vector3)wrapOffset;
    }
    // Fixed is called once per physics
    void FixedUpdate()
    {
        if (prep > 0)
        {
            prep--;
            if (prep == 1)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CameraTarget"))
                {
                    if (obj.name == "PlayerTank(Clone)")
                    {
                        if (target == null)
                        {
                            target = obj.transform;
                        }
                        else
                        {
                            target2 = obj.transform;
                        }
                    }
                }
                //set the target's wrap settings
                if (target2 != null)
                {
                    //2 player wrapping doesn't screenwrap
                    target2.gameObject.GetComponent<Wrap>().cameraTransform = null;
                }
            }
        }
        else if (follow == true)
        {
            if (target != null && target.GetComponent<MoveTank>().dead == false)
            {
                if (target2 != null && target2.GetComponent<MoveTank>().dead == false)
                {
                    //2 alive players
                    //find closest player 2 clone to player 1
                    Vector2 clonePos = (Vector2)target2.position;
                    float cloneDist = (target2.position - target.position).magnitude;
                    Vector2 newClonePos = clonePos + new Vector2(screenSize.x, 0f);
                    float newCloneDist = ((newClonePos - (Vector2)target.position).magnitude);
                    if (newCloneDist < cloneDist)
                    {
                        cloneDist = newCloneDist;
                        clonePos = newClonePos;
                    }
                    newClonePos = clonePos + new Vector2(-screenSize.x, 0f);
                    newCloneDist = ((newClonePos - (Vector2)target.position).magnitude);
                    if (newCloneDist < cloneDist)
                    {
                        cloneDist = newCloneDist;
                        clonePos = newClonePos;
                    }
                    newClonePos = clonePos + new Vector2(0f, screenSize.y);
                    newCloneDist = ((newClonePos - (Vector2)target.position).magnitude);
                    if (newCloneDist < cloneDist)
                    {
                        cloneDist = newCloneDist;
                        clonePos = newClonePos;
                    }
                    newClonePos = clonePos + new Vector2(0f, -screenSize.y);
                    newCloneDist = ((newClonePos - (Vector2)target.position).magnitude);
                    if (newCloneDist < cloneDist)
                    {
                        cloneDist = newCloneDist;
                        clonePos = newClonePos;
                    }
                    zoomTarget = Mathf.Max(cloneDist * 0.5f + 2f, initialZoomTarget - 1f);

                    transform.position = Vector3.SmoothDamp(transform.position,
                    (target.position + (Vector3)clonePos) * 0.5f + offset, ref velocity, smoothSpeed);
                }
                else
                {
                    zoomTarget = initialZoomTarget;
                    //p1 alive
                    transform.position = Vector3.SmoothDamp(transform.position,
                    target.position + offset, ref velocity, smoothSpeed);
                }
            }
            else if (target2 != null && target2.GetComponent<MoveTank>().dead == false)
            {
                zoomTarget = initialZoomTarget;
                // p2 alive
                if (target2.GetComponent<Wrap>().cameraTransform == null)
                {
                    //2 player wrapping now screenwraps
                    target2.GetComponent<Wrap>().cameraTransform = transform;
                }
                transform.position = Vector3.SmoothDamp(transform.position,
                target2.position + offset, ref velocity, smoothSpeed);
            }
            else
            {
                //noone alive
                follow = false;
                zoomTarget = Mathf.Max(screenSize.x, screenSize.y) * 0.5f + 0.2f;
            }
        }
        else
        {
            //follow is false
            transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(boss2offset2, boss2offset2 * 5, 0) + offset + (Vector3)wrapOffset, ref velocity, smoothSpeed);
        }
    }
}
