using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task4 : MonoBehaviour
{

    public Image dummy;
    public static Vector3 dummyStartPositionHorizontal;
    public static Vector3 dummyStartPositionVertical;

    public Image vertical;
    public Image horizontal;

    public static string direction;
    public static string directionDetail;


    void Start()
    {
        horizontal.enabled = true;
        vertical.enabled = false;
        direction = "horizontal";
        directionDetail = "right";
        dummyStartPositionHorizontal = new Vector3(-0.35f,1.2675f,-0.5f);
        dummyStartPositionVertical = new Vector3(-0.605f, 1.0075f, -0.5f);
        dummy.transform.position = dummyStartPositionHorizontal;
       
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("return")){
            if(direction == "horizontal"){
                horizontal.enabled = false;
                vertical.enabled = true;
                direction = "vertical";
                directionDetail = "up";
                dummy.transform.position = dummyStartPositionVertical;
            }
            else{
                horizontal.enabled = true;
                vertical.enabled = false;
                direction = "horizontal";
                directionDetail = "right";
                dummy.transform.position = dummyStartPositionHorizontal;
            }                
        }

        
        if(direction == "horizontal" && directionDetail == "right"){
            if(dummy.transform.position.x > -0.85){                    
                dummy.transform.Translate(0.0007f,0,0); 
            }
            else{
                directionDetail = "left";
            }                
        }
        else if(direction == "horizontal" && directionDetail == "left"){
            if(dummy.transform.position.x < -0.35){
                dummy.transform.Translate(-0.0007f,0,0); 
            }
            else{
                directionDetail = "right";
            }                
        }
        if(direction == "vertical" && directionDetail == "up"){ 
            if(dummy.transform.position.y < 1.51){
                dummy.transform.Translate(0,0.0007f,0); 
            }
            else{
                directionDetail = "down";
            }                
        }
        else if(direction == "vertical" && directionDetail == "down"){                
            if(dummy.transform.position.y > 1.0075){
                dummy.transform.Translate(0,-0.0007f,0); 
            }
            else{
                directionDetail = "up";
            }                
        }
        
    }

    public void onEnable(){
        Start();
    }
}
