using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragGrenade : bullet
{
    public GameObject Owner = null;
    public GameObject Frag = null;
    public float FragCount = 64;
    public float shotSpeed = 0.1f;

    public override void hit()
    {
        if (Owner != null)
        {
            Owner.GetComponent<Gun>().powerup = "none";
        }
        for (int i = 0; i < FragCount; i++)
        {
            GameObject p = Instantiate(Frag, transform.position, Quaternion.identity);
            float angle = i * 360f / (FragCount);
            p.GetComponent<Fragment>().velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle),
            Mathf.Sin(Mathf.Deg2Rad * angle)).normalized * shotSpeed;
            p.GetComponent<Fragment>().ownerScoreNumber = ownerScoreNumber;

        }
        Destroy(gameObject);
    }

    public override void SpecificAction()
    {
        if (Owner != null)
        {
            if (Owner.GetComponent<Gun>().powerup != "frag exploder")
            {
                hit();
            }
        }
        if (lifeLeft <= 1)
        {
            hit();
        }
    }
}
