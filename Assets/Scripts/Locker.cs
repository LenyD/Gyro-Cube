using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    public GameObject immovableChild;
    public GameObject node;

     private void OnTriggerEnter(Collider other) {
        //On collision, destroy the collider and itself, and show a immovable cube
        if(other.isTrigger){
			Destroy(other.gameObject);
			immovableChild.SetActive(true);
            Destroy(node);
        }
    }
}
