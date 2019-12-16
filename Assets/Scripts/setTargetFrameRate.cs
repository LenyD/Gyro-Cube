using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTargetFrameRate : MonoBehaviour
{
    void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 60;
    }

}
