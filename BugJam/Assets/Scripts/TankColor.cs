using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColor : MonoBehaviour
{
    public Gun gun;
    public SpriteRenderer sr;

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        sr.color = gun.tankColor;
    }
}
