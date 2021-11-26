using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupVisuals : MonoBehaviour
{
    public float maxLife = 500;

    public float fadeEnd = 400;
    public float fadeStart = 100;
    private float lifeLeft;
    public Color color = new Color(0f, 0f, 0f, 0f);

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
        lifeLeft -= 1;

        if (lifeLeft < fadeStart)
        {
            color.a = (lifeLeft / fadeStart);

            sr.color = new Color(0f, 0f, 0f, (lifeLeft / fadeStart));
        }

        if (lifeLeft > fadeEnd)
        {
            color.a = ((maxLife - fadeEnd) - (lifeLeft - fadeEnd)) / (maxLife - fadeEnd);
            sr.color = new Color(0f, 0f, 0f, ((maxLife - fadeEnd) - (lifeLeft - fadeEnd)) / (maxLife - fadeEnd));
        }
        sr.color = color;

    }
}
