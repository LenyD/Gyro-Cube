using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    protected Rigidbody rb;
    SoundEffect se;
    // Start is called before the first frame update
    public Gravity(){

    }
    void Awake() {
    }
    public void Start() {
        rb = this.GetComponent<Rigidbody>();
        se = this.GetComponent<SoundEffect>();
    }
    public bool getIsFalling(){
        //If the velocity is not close enough to zero, it's still considered falling
        if(rb==null){
            return false;
        }
        return (Mathf.Sqrt(rb.velocity.y * rb.velocity.y)>=0.01f);
    }
    public void OnDestroy() {
        //When destroyed, remove it from the list in FlipCube
        FlipCube.onDestroyGravCube(this);
    }
    public void OnCollisionEnter(Collision other) {
        //Play a sound if a collision is strong enough
        if(other.relativeVelocity.magnitude > 3f){
            se.playSound();
        }
    }
}
