using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Goal : MonoBehaviour
{
    private void Awake() {
    }
    private void Start() {
        Stat.addGoals(1);
        Stat.addGoalToList(this);
        Stat.setGoalColor();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.isTrigger && !(other.GetComponent<Gravity>() is AntiGravity)){
            //if it's reached
            goalReached();
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.isTrigger && !(other.GetComponent<Gravity>() is AntiGravity)){
            //If the cube moved out reverse the reached
            goalExited();
        }
    }
    void goalReached(){
        Stat.completedGoal(1);
        if(Stat.getGoalsToComplete()<=0 && Stat.getCoinsToGet()<=0){
            //If every goals are reach, activate the victory
            victory();
        }
    }
    void goalExited(){
        Stat.addGoals(1);
    }
    void victory(){
        string sceneName =SceneManager.GetActiveScene().name;
        int id = int.Parse(sceneName.Substring(5));
        //Save the completion of the level id to a file
        SaveManager.addCompletedLevel(id);
        //Activate the victory pop up
        BtActions btA = GameObject.Find("ButtonActionController").GetComponent<BtActions>();
        btA.victory();
    }
    
}
