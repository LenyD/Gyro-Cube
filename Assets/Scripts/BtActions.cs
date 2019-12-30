using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BtActions:MonoBehaviour
{
    int currentId;
    public Text levelId;//Level # Textfield
    public Animation a;//Transition animation
    public GameObject vicPopUp;//Victory pop up
    public Animator stuckText;
    public MenuButtons levelList;
    bool isTransitionning = false;
    GameObject currentPopUp;
    void Awake() {
        if(levelId!=null){
            //If there is a Text linked
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = SceneManager.GetActiveScene().name;
            //get the level number from the scene name
            string id = sceneName.Substring(5);
            levelId.text = "#"+id;
            currentId = int.Parse(id);
            stuckText.speed = 0;
        }
    }

    public void resetScene() {
        //Called from scene button
        if(isTransitionning){
            return;
        }
        isTransitionning = true;
        StartCoroutine(reset());
    }
    IEnumerator reset(){
        //Reload scene after transition
        a.PlayQueued("CloseScene");
        yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        Stat.resetAll();
        SceneManager.LoadScene(currentScene.name,LoadSceneMode.Single);
    }
    public void nextLevel() {
        //Called from scene button
        if(isTransitionning){
            return;
        }
        isTransitionning = true;
        StartCoroutine(next());
    }
    IEnumerator next(){
        //Load next scene or menu if it's the last level after transition
        a.PlayQueued("CloseScene");
        yield return new WaitForSeconds(1f);
        currentId++;
        string nextSceneName = "Level"+currentId;
        if(Application.CanStreamedLevelBeLoaded(nextSceneName)){
            SceneManager.LoadScene(nextSceneName);
        }else{
            SceneManager.LoadScene("Menu");
        }
    }

    public void toMenuScene() {
        //Called from scene button
        if(isTransitionning){
            return;
        }
        isTransitionning = true;
        StartCoroutine(toMenu());
    }
    IEnumerator toMenu(){
        //Go to menu after transition
        a.PlayQueued("CloseScene");
        yield return new WaitForSeconds(1f);
        Stat.resetAll();
        SceneManager.LoadScene("Menu",LoadSceneMode.Single);
    }
    public void victory(){
        //Enable the victory pop up with the next level button
        showPopUp(vicPopUp);
    }
    public void closeGame(){
        if(isTransitionning){
            return;
        }
        isTransitionning = true;
        //Called from button in main menu scene
        StartCoroutine(quit());
    }
    IEnumerator quit(){
        //Go to menu after transition
        a.PlayQueued("CloseScene");
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    public void resetProgress() {
        //Called from scene button
        SaveManager.resetProgress();
        StartCoroutine(toMenu());
        //levelList.refreshList();
    }
    public void showStuckText(){
        if(stuckText!=null){
            stuckText.speed = 1;
        }
    }
    public void showPopUp(GameObject popUp){
        if(currentPopUp != null){
            currentPopUp.SetActive(false);
        }
        currentPopUp = popUp;
        popUp.SetActive(true);
    }
    public void hidePopUp(GameObject popUp){
        if(currentPopUp != null){
            currentPopUp.SetActive(false);
        }
        currentPopUp = popUp;
        popUp.SetActive(false);
    }
}
