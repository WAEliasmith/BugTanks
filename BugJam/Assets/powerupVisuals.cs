using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupVisuals : MonoBehaviour
{
    public int maxLife = 500;

    public int fadeEnd = 400;
    public int fadeStart = 100;
    private int lifeLeft;

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        lifeLeft = maxLife;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (lifeLeft < maxLife)
        {
            sr.color = new Color(0f, 0f, 0f, 1f);
        }
        lifeLeft -= 1;

        if (lifeLeft < fadeStart)
        {
            sr.color = new Color(0f, 0f, 0f, (lifeLeft / fadeStart) + 0.1f);
        }

        if (lifeLeft > fadeEnd)
        {
            sr.color = new Color(0f, 0f, 0f, ((maxLife - fadeEnd) - (lifeLeft - fadeEnd)) / (maxLife - fadeEnd) + 0.1f);
        }
    }
}
