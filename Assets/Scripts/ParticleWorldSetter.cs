using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWorldSetter : MonoBehaviour
{
    public ParticleSystem.MainModule ps_main;
    // Start is called before the first frame update
    private void Awake() {
        //Set spaceSimulation to the FlipCube
        ps_main = GetComponent<ParticleSystem>().main;
        ps_main.customSimulationSpace = GameObject.Find("Cube").GetComponent<Transform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
