using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stat
{
    //Static class that controls
    static int currentCoin = 0;//Number of coin in the level
    static int goalsToComplete = 0;//Goals left to reach
    static int numberOfMoves = 0;//Number of flip
    static List<Goal> goals;//List of the goals objects
    public static void resetAll(){
        //Reset all stats
        resetCurrentCoin();
        resetGoal();
        resetGoalList();
        resetNumberOfMoves();
    }
    public static void addCoins(int num){
        //increase the number of coins needed
        currentCoin+=num;
        if(currentCoin>0){
            //Set the goal color to yellow
            setGoalColor("yellow");
        }
    }
    public static void removeCoins(int num){
        //Reduce the number of coins needed
        currentCoin-=num;
        if(currentCoin<=0){
            //If there is no coin left set the goal to red
            setGoalColor("red");
        }
    }
    public static void resetCurrentCoin(){
        //Reset value
        currentCoin = 0;
    }
    public static void addGoals(int num){
        //Add to the number of goals to reach
        goalsToComplete+=num;
    }
    public static void completedGoal(int num){
        //Reduce goals to complete when it's reached
        goalsToComplete-=num;
    }
    public static void resetGoal(){
        //reset value
        goalsToComplete=0; 
    }
    public static int getGoalsToComplete(){
        //getter
        return goalsToComplete;
    }
    public static int getCoinsToGet(){
        //getter
        return currentCoin;
    }
    public static int incrementNumberOfMoves(){
        //Increment the number of move after every rotation
        numberOfMoves++;
        return numberOfMoves;
    }
    public static void resetNumberOfMoves(){
        //Reset number of move
        numberOfMoves = 0;
    }
    public static void addGoalToList(Goal g){
        //Add a goal to the list
        goals.Add(g);
    }
    public static void resetGoalList(){
        //Empty the list of goals
        goals = new List<Goal>();
    }
    public static void setGoalColor(string color=null){
        //Set every goals color from string
        Color c;
        if(color==null){
            //If no string given, select color from the number of coins to collect
            if(getCoinsToGet()>0){
                color = "yellow";
            }else{
                color = "red";
            }
        }
        switch(color){
            case "yellow":
            case "Yellow":
            case "y":
            case "Y":c = new Color(.5f,.5f,0.1f);;break;
            case "red":
            case "Red":
            case "r":
            case "R":
            default:c = new Color(.1f,.05f,0.05f);;break;
        }

        foreach (var goal in goals)
        {
            //Apply the color to the _emissioncolor
            goal.GetComponent<Renderer>().material.SetColor("_EmissionColor",c*1f);
        }
    }
}
