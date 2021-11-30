using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisuals : MonoBehaviour
{
    public Gun Gun;
    public Sprite[] spriteArray;

    private SpriteRenderer sr;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (Gun.powerup)
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

        if (Gun.powerup == "none" && Gun.crisp == true)
        {
            sr.sprite = spriteArray[12];
        }
    }
}
