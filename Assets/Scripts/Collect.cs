using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private void Start() {
        //Add the coin to the collect list
        Stat.addCoins(1);
    }
   private void OnTriggerEnter(Collider other) {
       collected();
   }
   void collected(){
       //Remove collider to stop multiple collision on the same coin
       GetComponent<Collider>().enabled = false;
       //Remove from the collect list
       Stat.removeCoins(1);
       //Destroy it
       Destroy(this.gameObject);
   }
}
