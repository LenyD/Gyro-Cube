using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Button prefabButton;
    List<Button> btList = new List<Button>();
    public int nbLevel = 25;//Number of button to create
    float x = 100;//width
    float y = -100;//height
    float margin = 100;//Margin
    int nbBtPerRow = 5;//Number of button per row
    int levelPerPage = 24;
    RectTransform rt;
    Animation transition;
    public int currentPage = 1;
    public GameObject prev,next;
    void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Animation>();
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x,600);
        //Set up the level list
        refreshList(currentPage);
    }
    void createButton(float x, float y,int id){
        //Instantiate  button and set it up with the events
        Button newBt;
        newBt = (Button)Instantiate(prefabButton);
        newBt.GetComponentInChildren<Text>().text = id.ToString();
        newBt.transform.SetParent(this.transform);
        newBt.transform.localScale = new Vector3(1,1,1);
        newBt.transform.localPosition = new Vector3(x,y,-1);
        newBt.onClick.AddListener(delegate {loadLevel(id);});
        newBt.transform.rotation = transform.parent.transform.rotation;
        //Hide a check if the level is not completed according to the saveFile
        newBt.transform.Find("Checked").GetComponent<Text>().enabled = SaveManager.isLevelCompleted(id);
        btList.Add(newBt);
    }
    public void loadLevel(int levelNumber){
        StartCoroutine(loadLevelCoroutine(levelNumber));
    }
    IEnumerator loadLevelCoroutine(int levelNumber){
        //Load level X after transition on button pressed
        transition.PlayQueued("CloseScene");
        yield return new WaitForSeconds(1f);
        Stat.resetAll();
        try{
            SceneManager.LoadScene("Level"+levelNumber);

        }catch(UnityException e){
            SceneManager.LoadScene("Menu");
        }
    }
    void showPrevPageBt(){
        prev.SetActive(true);
    }
    void showNextPageBt(){
        next.SetActive(true);
    }
    public void showPrevPage(){
        currentPage--;
        refreshList(currentPage);
    }
    public void showNextPage(){
        currentPage++;
        refreshList(currentPage);
    }

    public void refreshList(int page = 1){
        foreach (Button b in btList)
        {
            Destroy(b.gameObject);
        }
        prev.SetActive(false);
        next.SetActive(false);
        btList = new List<Button>();
        int i = 2;
        int mod = page - 1;
        if(page==1){
            i=1;
        }else{
            showPrevPageBt();
        }
        if(nbLevel>page*levelPerPage+1){
            showNextPageBt();
        }
        while ( i <= levelPerPage)
        {
            int id =i+(page*levelPerPage)-(levelPerPage)-mod;
            
            //Create every button and position them on the grid
            if(id <= nbLevel){
                createButton((i-1)%nbBtPerRow*x+margin ,Mathf.RoundToInt((i-1)/nbBtPerRow)*y-margin,id);
            }
            i++;
        }
    }
}