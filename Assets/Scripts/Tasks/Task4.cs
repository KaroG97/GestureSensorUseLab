using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task4 : MonoBehaviour
{

    public GameObject verticalParent;
    public GameObject horizontalParent; 

    public Slider vertical;
    public Slider horizontal;

    public static string direction;
    public static string directionDetail;

    public static bool wholeNumbers; 

    public float timeBetweenSteps;



    void Start()
    {
        horizontalParent.SetActive(true);
        verticalParent.SetActive(false);
        direction = "horizontal";
        directionDetail = "right";
        wholeNumbers = false;   
        timeBetweenSteps = 1.5f;    
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("return")){
            if(direction == "horizontal" && wholeNumbers == false){
                print("1");
                horizontalParent.SetActive(false);
                verticalParent.SetActive(true);
                direction = "vertical";
                directionDetail = "up";
            }
            else if(direction == "vertical" && wholeNumbers == false){
                print("2");
                horizontalParent.SetActive(false);
                verticalParent.SetActive(true);
                direction = "vertical";                
                directionDetail = "up";
                wholeNumbers = true;
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;
            }                
            else if(direction == "horizontal" && wholeNumbers == true){
                print("3");
                horizontalParent.SetActive(true);
                verticalParent.SetActive(false);
                wholeNumbers = false;
                direction = "horizontal";
                directionDetail = "right";
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;
            }
            else if(direction == "vertical" && wholeNumbers == true){
                print("4");
                horizontalParent.SetActive(true);
                verticalParent.SetActive(false);
                direction = "horizontal";
                directionDetail = "right";
                wholeNumbers = true;
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;
            }
        } 

        if(wholeNumbers == true){
            if(timeBetweenSteps > 0){
                timeBetweenSteps -= Time.deltaTime;
            }
            else{            
                if(direction == "horizontal" && directionDetail == "right"){
                    if(horizontal.value < horizontal.maxValue){
                    horizontal.value = horizontal.value + 1; 
                    }
                    else{
                        directionDetail = "left";
                    }
                }
                else if(direction == "horizontal" && directionDetail == "left"){
                    if(horizontal.value > horizontal.minValue){
                    horizontal.value = horizontal.value - 1; 
                    }
                    else{
                        directionDetail = "right";
                    }
                }
                else if(direction == "vertical" && directionDetail == "up"){
                    if(vertical.value < vertical.maxValue){
                    vertical.value = vertical.value + 1; 
                    }
                    else{
                        directionDetail = "down";
                    }
                }
                else if(direction == "vertical" && directionDetail == "down"){
                    if(vertical.value > vertical.minValue){
                    vertical.value = vertical.value - 1; 
                    }
                    else{
                        directionDetail = "up";
                    }
                }
                timeBetweenSteps = 1.5f;
            }

        }   
        else if(wholeNumbers == false){
            if(direction == "horizontal" && directionDetail == "right"){
                if(horizontal.value < horizontal.maxValue){
                   horizontal.value = horizontal.value + 0.02f; 
                }
                else{
                    directionDetail = "left";
                }
            }
            else if(direction == "horizontal" && directionDetail == "left"){
                if(horizontal.value > horizontal.minValue){
                   horizontal.value = horizontal.value - 0.02f; 
                }
                else{
                    directionDetail = "right";
                }
            }
            else if(direction == "vertical" && directionDetail == "up"){
                if(vertical.value < vertical.maxValue){
                   vertical.value = vertical.value + 0.02f; 
                }
                else{
                    directionDetail = "down";
                }
            }
            else if(direction == "vertical" && directionDetail == "down"){
                if(vertical.value > vertical.minValue){
                   vertical.value = vertical.value - 0.02f; 
                }
                else{
                    directionDetail = "up";
                }
            }
        }    
    }

    public void onEnable(){
        Start();
    }
}
