using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

[Serializable]
public class LightTower : GridEntity
{


    [HideInInspector] public Light2D Light;


    public override void Start()
    {
        Light.gameObject.SetActive(false);
        Light.gameObject.transform.localPosition = Vector3.zero;
    }
    public override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) // Detects Spacebar press
        {
            this.GetDamaged(41);
        }
    }
    public override void ConstructionComplete()
    {
        Light.gameObject.SetActive(true);
    }
}
