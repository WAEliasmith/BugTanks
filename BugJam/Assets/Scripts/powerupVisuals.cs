using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupVisuals : MonoBehaviour
{
    public Powerup powerup;

    public float fadeInTime = 100f;
    public float fadeEnd = 400;
    public float fadeStart = 100;
    public Color color = new Color(0f, 0f, 0f, 0f);

    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        fadeEnd = powerup.maxLife - fadeInTime;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // FixedUpdate is called once per physics
    void FixedUpdate()
    {
        if (powerup.lifeLeft < fadeStart)
        {
            color.a = (powerup.lifeLeft / fadeStart);

            sr.color = new Color(0f, 0f, 0f, (powerup.lifeLeft / fadeStart));
        }

        if (powerup.lifeLeft > fadeEnd)
        {
            color.a = ((powerup.maxLife - fadeEnd) - (powerup.lifeLeft - fadeEnd)) / (powerup.maxLife - fadeEnd);
            sr.color = new Color(0f, 0f, 0f, ((powerup.maxLife - fadeEnd) - (powerup.lifeLeft - fadeEnd)) / (powerup.maxLife - fadeEnd));
        }
        sr.color = color;

    }
}
