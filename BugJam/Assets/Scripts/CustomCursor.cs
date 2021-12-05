using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomCursor : MonoBehaviour
{
    public float clickStretch = 0.3f;
    public float clickedRecently = 10f;
    private float clickedRecentlyLeft;
    public Vector3 offset;
    private Vector3 worldPosition;
    public float maxForce = 10f;
    public float forceMultiplier = 1f;
    public float holdDelay = 6;
    public float holdDelayLeft;
    public bool holdMode = false;
    public Image img;


    void Start()
    {
        Cursor.visible = false;
    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedRecentlyLeft = clickedRecently;
        }
    }
    void Update()
    {
        transform.SetAsLastSibling();
        transform.localScale =
        new Vector3(clickStretch * clickedRecentlyLeft / clickedRecently + 1f,
        clickStretch * clickedRecentlyLeft / clickedRecently + 1f, 1f);
        if (clickedRecentlyLeft > 0)
        {
            clickedRecentlyLeft--;
        }

        transform.position = Input.mousePosition;

        if (SceneManager.GetActiveScene().name == "TitleScreen" || MenuManager.instance.pause == true)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
