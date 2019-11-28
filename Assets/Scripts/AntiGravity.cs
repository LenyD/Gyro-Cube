using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravity : Gravity
{
    
    Vector3 reverseGForce = new Vector3(0,9.81f,0);
    public AntiGravity(){

    }
    void FixedUpdate()
    {
        //Reversed gravity * weight*1.5 so it can lift gravity blocks
        rb.AddForce(reverseGForce*15);
    }
}
