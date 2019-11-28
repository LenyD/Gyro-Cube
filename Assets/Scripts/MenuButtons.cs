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
    RectTransform rt;
    Animation transition;

    void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Animation>();
        rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x,Mathf.Round(nbLevel/nbBtPerRow)*y*-1+(margin*2));
        //Set up the level list
        for (int i = 1; i <= nbLevel; i++)
        {
            //Create every button and position them on the grid
            createButton((i-1)%nbBtPerRow*x+margin ,Mathf.RoundToInt((i-1)/nbBtPerRow)*y-margin,i);
        }
        SaveManager.isLevelCompleted(5);
    }
    void createButton(float x, float y,int id){
        //Instantiate  button and set it up with the events
        Button newBt;
        newBt = (Button)Instantiate(prefabButton);
        newBt.GetComponentInChildren<Text>().text = id.ToString();
        newBt.transform.SetParent(this.transform);
        newBt.transform.localScale = new Vector3(1,1,1);
        newBt.transform.localPosition = new Vector3(x,y,0);
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
        SceneManager.LoadScene("Level"+levelNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
