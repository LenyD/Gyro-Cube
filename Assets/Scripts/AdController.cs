using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{
    private string storeId = "3403931";
    private string videoAdId = "video";

    // Start is called before the first frame update
    void Start()
    {
        //Advertisement.Initialize(storeId,true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)){
            /*if(Advertisement.IsReady(videoAdId)){
                
            }*/
        }
    }
}
