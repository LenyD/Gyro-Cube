using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCube : MonoBehaviour
{
    //Target rotation for every sides  0 UP        1 Down               2 Right              3 Left               4 counter-clock    5 clockwise
    Vector3[] predefinedRot = {new Vector3(90,0,0),new Vector3(-90,0,0),new Vector3(0,-90,0),new Vector3(0,90,0),new Vector3(0,0,90),new Vector3(0,0,-90)};
    Vector3 targetRot = new Vector3(), //Target value to reach
    rotation = new Vector3(), //Increment in rotation during the animation
    mpStart, mpStop;//Mouse position on down and up
    List<Gravity> gravCubes = new List<Gravity>();//List of every gravcubes and antigrav cube
    bool isRotating = false, isAbleToRotate = false, isClicked = false;
    float rotSpeed =10f;//Rotation speed
    int nextRotation = -1,//Next rotation id from predefinedRot, -1 is none
    horizontalSection,verticalSection;

    Renderer rend;
    ParticleSystem touch;//Touch feed back prefab
    Transform container,arrow,clockwise;//Child object that need to be transformed
    public Vector2 tiling = new Vector2(5,5);//Number of subdivision in the mat
    Vector2 res;//Resolution to move the touch feedback at the right place
    BtActions UIManager;
    private void Awake() {
        Stat.resetAll();//Reset stats
        //Get object and childs from the scene
        touch = GameObject.Find("Touch").GetComponent<ParticleSystem>();
        UIManager = GameObject.Find("ButtonActionController").GetComponent<BtActions>();
        arrow = touch.gameObject.transform.Find("arrow");
        clockwise = touch.gameObject.transform.Find("clock");
        res = new Vector3(Screen.width,Screen.height,0);
        //get the current resolution
    }
    // Start is called before the first frame update
    void Start()
    {
        if(tiling.x<=0&&tiling.y<=0){
            //if the tilling is more than 0 stop to dodge error
            return;
        }
        //Get the child transfrom containing every puzzle pieces 
        container = this.gameObject.transform.GetChild(0);

        rend = GetComponent<Renderer>();
        //Caculate scale according to the tillings value
        float newScale = (5/tiling.x)-(.0005f/tiling.x);
        container.localScale = new Vector3(newScale,newScale,newScale);
        Gravity[] grav= FindObjectsOfType<Gravity>();
        //Set the mat to have the good number of tiles
        rend.material.mainTextureScale = tiling;
        //Fills up the gravCubes list
        AntiGravity[] antiGrav= FindObjectsOfType<AntiGravity>();
        foreach (var g in grav)
        {
            gravCubes.Add(g);
        }
        foreach (var a in antiGrav)
        {
            gravCubes.Add((Gravity)a);
        }
        
    }
    public void setIsRotating(bool b){
        //Setter
        isRotating = b;
    }
    public void refreshGravCubes(Gravity cube){
        //Remove a cube given in parameter from the list
        List<Gravity> newGravCubes = new List<Gravity>();
        foreach (var g in gravCubes)
        {
            if( cube.Equals(g)){
                newGravCubes.Remove(g);
            }
        }
    }
    public static void onDestroyGravCube(Gravity cube){
        //Called from the GravCube class when destroyed to refresh the list
        FlipCube fc = FindObjectOfType<FlipCube>();
        if(fc!=null){
            //Only refresh if this still exist
            fc.refreshGravCubes(cube);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Get rotation if there is a swipe
        nextRotation = getSwipeInput();
        //enable Cube rotation
        if(Vector3.Distance(targetRot, rotation)<1f){
            isRotating = false;
        }
        //If is not already rotating, listen for nextRotation of keyboard input
        if(!isRotating){
            //If you can rotate(no cubes are still falling)
            if(isAbleToRotate){
                if(nextRotation>=0){
                    rotatePuzzle(predefinedRot[nextRotation]);
                }else if(Input.GetKeyUp("up")||Input.GetKeyUp("w")){
                    rotatePuzzle(predefinedRot[0]);
                }else if(Input.GetKeyUp("down")||Input.GetKeyUp("s")){
                    rotatePuzzle(predefinedRot[1]);
                }else if(Input.GetKeyUp("right")||Input.GetKeyUp("d")){
                    rotatePuzzle(predefinedRot[2]);
                }else if(Input.GetKeyUp("left")||Input.GetKeyUp("a")){
                    rotatePuzzle(predefinedRot[3]);
                }else if(Input.GetKeyUp("q")){
                    rotatePuzzle(predefinedRot[4]);
                }else if(Input.GetKeyUp("e")){
                    rotatePuzzle(predefinedRot[5]);
                }   
            }else{
                //Check if the cubes are done falling
                foreach (var g in gravCubes)
                {
                    //Block rotation if a cube is still falling
                    isAbleToRotate = !g.getIsFalling();
                    if(!isAbleToRotate){
                        break;
                    }
                }
                if(gravCubes.Count == 0){
                    isAbleToRotate = true;
                }
            }
        }
        //Set nextRotation to -1 after sending it
        nextRotation = -1;
    }

    int getSwipeInput(){
        //Analyse swipe direction and return it to nextRotation
        int dir=-1;
        if(Input.GetMouseButtonDown(0)){
            //On click down get start position ans move touch feedback
            mpStart= Input.mousePosition;
            res = new Vector3(Screen.width,Screen.height,0);
            isClicked = true;
            Vector3 newPos = mpStart/res;
            //Set touch particle position to ratio with deformation
            newPos.x*=10.6f;
            newPos.y*=6;
            newPos += new Vector3(-5.3f,-3,-4f);
            horizontalSection = Mathf.RoundToInt(newPos.x/Mathf.Abs(newPos.x));
            verticalSection = Mathf.RoundToInt(newPos.y/Mathf.Abs(newPos.y));
            if(Mathf.Abs(newPos.x)>Mathf.Abs(newPos.y)){
                if(Mathf.Abs(newPos.y)<1.5f){
                    verticalSection = 0;
                }
            }
            if(Mathf.Abs(newPos.x)<Mathf.Abs(newPos.y)){
                if(Mathf.Abs(newPos.x)<1.5f){
                    horizontalSection = 0;
                }
            }
            newPos.x = horizontalSection*2.4f;
            newPos.y = verticalSection*2.4f;
            //Move position under cursor and start
            touch.transform.position = newPos ;
            touch.Play();
            //convert mpStart to cursor position lockon
            mpStart = newPos -=new Vector3(-5.3f,-3,-4f);
            mpStart.x/=10.6f;
            mpStart.y/=6;
            mpStart*=res;
        }
        if(isClicked){
            mpStop= Input.mousePosition;
            if(Vector3.Distance(mpStart,mpStop)>500){
                Vector2 direction = mpStart - mpStop;
                if(Mathf.Abs(direction.y)>Mathf.Abs(direction.x)){
                    if(direction.y<0){
                        //bottom
                        if(horizontalSection<0){
                            dir = 5;
                        }else if(horizontalSection>0){
                            dir = 4;
                        }else{
                            dir = 0;
                        }
                    }else{
                        //up
                        if(horizontalSection<0){
                            dir = 4;
                        }else if(horizontalSection>0){
                            dir = 5;
                        }else{
                            dir = 1;
                        }
                    }
                }else{
                    if(direction.x<0){
                        //left
                        if(verticalSection<0){
                            dir = 4;
                        }else if(verticalSection>0){
                            dir = 5;
                        }else{
                            dir = 2;
                        }
                    }else{
                        //right
                        if(verticalSection<0){
                            dir = 5;
                        }else if(verticalSection>0){
                            dir = 4;
                        }else{
                            dir = 3;
                        }
                    }
                }
            }
            if(Input.GetMouseButtonUp(0)){
                //On release stop the touch feedback
                touch.Stop();
                isClicked = false;
                arrow.gameObject.SetActive(false);
                clockwise.gameObject.SetActive(false);
                //Return final direction
                return dir;
            }
            switch(dir){
                case 0: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,90);break;
                case 1: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,270);break;
                case 2: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,0);break;
                case 3: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,180);break;
                case 4: arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(true);clockwise.localScale = new Vector3(-2,2,0);break;
                case 5: arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(true);clockwise.localScale = new Vector3(2,2,0);break;
                default:arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(false);break;
            }
        }
        return -1;
        /*
        if(Input.GetMouseButtonDown(0)){
            //On click down get start position ans move touch feedback
            mpStart= Input.mousePosition;
            res = new Vector3(Screen.width,Screen.height,0);
            isClicked = true;
            Vector3 newPos = mpStart/res;
            //Set touch particle position to ratio with fov
            newPos.x*=10.6f;
            newPos.y*=6;
            newPos += new Vector3(-5.3f,-3,-4f);
            //Move position under cursor and start
            touch.transform.position = newPos ;
            touch.Play();
        }
        if(isClicked){
            //Calculate direction according to the mouse position startingPoint
            mpStop= Input.mousePosition;
            if(Vector3.Distance(mpStart,mpStop)>200){
                Vector2 direction = mpStart - mpStop;
                Debug.Log(mpStart);
                //Diag
                if(Mathf.Abs(Mathf.Abs(direction.x)-Mathf.Abs(direction.y))<= 150){
                    if(direction.x<0){
                        if(direction.y<0){
                            //up left
                            dir = 4;
                        }else{
                            //bottom left
                            dir = 5;
                        }
                    }else{
                        if(direction.y<0){
                            //up right
                            dir = 5;
                        }else{
                            //bottom right
                            dir = 4;
                        }
                    }
                }else if(Mathf.Abs(direction.y)>Mathf.Abs(direction.x)){
                    if(direction.y<0){
                        //bottom
                        dir = 0;
                    }else{
                        //up
                        dir = 1;
                    }
                }else{
                    if(direction.x<0){
                        //left
                        dir = 2;
                    }else{
                        //right
                        dir = 3;
                    }
                }
            }
            if(Input.GetMouseButtonUp(0)){
                //On release stop the touch feedback
                touch.Stop();
                isClicked = false;
                arrow.gameObject.SetActive(false);
                clockwise.gameObject.SetActive(false);
                //Return final direction
                return dir;
            }
            //Set direction of arrow according to the current direction pointed
            switch(dir){
                case 0: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,90);break;
                case 1: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,270);break;
                case 2: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,0);break;
                case 3: arrow.gameObject.SetActive(true);clockwise.gameObject.SetActive(false);arrow.rotation = Quaternion.Euler(0,0,180);break;
                case 4: arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(true);clockwise.localScale = new Vector3(-2,2,0);break;
                case 5: arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(true);clockwise.localScale = new Vector3(2,2,0);break;
                default:arrow.gameObject.SetActive(false);clockwise.gameObject.SetActive(false);break;
            }

        }
        return -1;
        */
        //Return no direction if there is no click
    }
    void FixedUpdate() {
        //Rotate the cube toward the target direction
        rotation = targetRot*rotSpeed*Time.deltaTime;
        targetRot -= rotation;
        transform.Rotate(rotation,Space.World);
    }
    void rotatePuzzle(Vector3 rot){
        //Set the target rotation to start the fixed update rotation
        isRotating = true;
        isAbleToRotate = false;
        if(Stat.incrementNumberOfMoves()==35){
            UIManager.showStuckText();
        }
        targetRot+= rot;
    }
}


