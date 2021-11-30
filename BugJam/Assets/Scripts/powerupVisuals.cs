using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupVisuals : MonoBehaviour
{
    public Powerup powerup;
    public Sprite[] spriteArray;


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
        switch (powerup.powerup)
        {
            case "lazer":
                sr.sprite = spriteArray[1];
                break;
            case "weird":
                sr.sprite = spriteArray[2];
                break;
            case "rpg":
                sr.sprite = spriteArray[3];
                break;
            case "grenade":
                sr.sprite = spriteArray[4];
                break;
            case "frag shot":
                sr.sprite = spriteArray[5];
                break;
            case "absorb shot":
                sr.sprite = spriteArray[6];
                break;
            case "wifi missile":
                sr.sprite = spriteArray[7];
                break;
            case "missile":
                sr.sprite = spriteArray[8];
                break;
            case "frag exploder":
                sr.sprite = spriteArray[9];
                break;
            case "shockwave":
                sr.sprite = spriteArray[10];
                break;
            case "wings":
                sr.sprite = spriteArray[11];
                break;
            default:
                sr.sprite = spriteArray[0];
                break;
        }
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
