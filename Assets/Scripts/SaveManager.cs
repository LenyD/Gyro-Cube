using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    
    static string path = Application.persistentDataPath +"/completedLevels.txt";
    public static void addCompletedLevel(int n){
        //Add the level id to the file of completed level if it's not already there
        if(isLevelCompleted(n)){
            return;
        }
        File.AppendAllText(path,n.ToString()+"\n");
   
    }
    public static StreamReader reader;
    public static bool isLevelCompleted(int n){
        //Search if the level is completed of not
        //If the file and directories are not created, create them
        if(!File.Exists(path)){
            if(!Directory.Exists(Application.persistentDataPath)){
                DirectoryInfo d = Directory.CreateDirectory(Application.persistentDataPath);
            }
            FileStream f= File.Create(path);
            f.Close();
        }
        //Read each line and return true if found
        reader = new StreamReader(path);
        string line = reader.ReadLine();
        while(line != null){
            if(line == n.ToString()){
                reader.Close();
                return true;
            }
            line = reader.ReadLine();
        }
        reader.Close();
        
        return false;
    }
}
