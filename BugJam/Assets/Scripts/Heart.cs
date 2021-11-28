using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public string player = "";
    public int heart = 1;
    public hurtbox hbox = null;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CameraTarget"))
        {
            if (obj.name == player)
            {
                hbox = FindGameObjectInChildWithTag(obj, "Player").GetComponent<hurtbox>();
            }
        }

        transform.position += new Vector3(60 * heart, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hbox && hbox.hp < heart)
        {
            gameObject.SetActive(false);
        }
    }

    public GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }
}
