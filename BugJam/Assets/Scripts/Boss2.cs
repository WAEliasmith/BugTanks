using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public float reload1 = 20;
    public float reload2 = 40;
    public float reload3 = 180;
    public float reload1Left = 20;
    public float reload2Left = 40;
    public float reload3Left = 180;
    public Gun gun;

    public MoveTank movement = null;
    public bool goTime = true;

    public GameObject airRaid = null;

    public int heart1 = 1;
    public int heart2 = 3;
    public int heart3 = 5;
    public hurtbox hbox = null;
    void FixedUpdate()
    {
        movement.xAxis = 1f;
        reload1Left--;
        reload2Left--;
        reload3Left--;
        if (reload1Left <= 0)
        {
            reload1Left = reload1;
            gun.powerup = "none";
            gun.Shoot();
        }
        if (reload2Left <= 0 && hbox.hp < heart1)
        {
            reload2Left = reload2;
            gun.powerup = "absorb shot";
            gun.Shoot();
        }
        if (reload3Left <= 0 && hbox.hp < heart2)
        {
            reload3Left = reload3;
            reload2Left = reload2;
            reload1Left = reload1 * 8;
            gun.powerup = "frag shot";
            gun.Shoot();
        }

        if (hbox && hbox.hp <= heart3 && goTime)
        {
            goTime = false;
            for (int i = 0; i < 10; i++)
            {
                GameObject p = Instantiate(airRaid, new Vector3(i, -4.3f, 0f), Quaternion.identity);
                p.GetComponent<powerupWillSpawn>().timer += -20 + i * 5;

            }
        }
    }
}
