using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToControlTimer : MonoBehaviour
{
    float msForHint = 10f,
    currentMs = 0f;
    public Animator hint;
    // Update is called once per frame
    private void Awake() {
        hint.speed = 0;
    }
    void Update()
    {
        
        currentMs+= Time.deltaTime;
        Debug.Log(currentMs);
        if(currentMs>msForHint){
            hint.gameObject.SetActive(true);
            hint.speed=1;
        }
        if(Input.GetMouseButtonUp(0)){
            currentMs = 0;
            hint.speed=0;
            hint.gameObject.SetActive(false);
        }
    }
    
}
