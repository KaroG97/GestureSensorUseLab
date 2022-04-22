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

    public Color activeColor;
    public Color inactiveColor;

    public static string direction;
    public static string directionDetail;

    public static bool wholeNumbers; 

    public float timeBetweenSteps;
    public float activationTime;



    void Start()
    {
        /*horizontalParent.SetActive(true);
        verticalParent.SetActive(false);
        direction = "horizontal";
        directionDetail = "right";
        wholeNumbers = false;   
        timeBetweenSteps = 1.5f;
        activationTime = 3.0f;  
        activeColor = new Color(0f/255f, 208f/255f, 255f/255f);
        inactiveColor = horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color; */
        horizontalParent.SetActive(false);
        verticalParent.SetActive(true);
        direction = "vertical";
        directionDetail = "down";
        wholeNumbers = false;   
        timeBetweenSteps = 1.5f;
        activationTime = 3.0f;  
        activeColor = new Color(0f/255f, 208f/255f, 255f/255f);
        inactiveColor = horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color; 
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("return")){
            if(direction == "horizontal" && wholeNumbers == false){
                horizontalParent.SetActive(false);
                verticalParent.SetActive(true);
                direction = "vertical";
                directionDetail = "up";                
                horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
            }
            else if(direction == "vertical" && wholeNumbers == false){
                horizontalParent.SetActive(false);
                verticalParent.SetActive(true);
                direction = "vertical";                
                directionDetail = "up";
                wholeNumbers = true;
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;                
                horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
            }                
            else if(direction == "horizontal" && wholeNumbers == true){
                horizontalParent.SetActive(true);
                verticalParent.SetActive(false);
                wholeNumbers = false;
                direction = "horizontal";
                directionDetail = "right";
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;
                horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
            }
            else if(direction == "vertical" && wholeNumbers == true){
                horizontalParent.SetActive(true);
                verticalParent.SetActive(false);
                direction = "horizontal";
                directionDetail = "right";
                wholeNumbers = true;
                horizontal.wholeNumbers = wholeNumbers;
                vertical.wholeNumbers = wholeNumbers;
                horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
            }
        } 

        if(wholeNumbers == true){
            if(timeBetweenSteps > 0){
                timeBetweenSteps -= Time.deltaTime;
            }
            else{  
                if(timeBetweenSteps > 1.0f){
                    horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                    vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = inactiveColor;
                }    
                else{
                    horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = activeColor;
                    vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = activeColor; 
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
                        vertical.value = vertical.value + 1 ; 
                        }
                        else{
                            directionDetail = "down";
                        }
                    }
                    else if(direction == "vertical" && directionDetail == "down"){
                        if(vertical.value > vertical.minValue){
                        vertical.value = vertical.value - 1 ; 
                        }
                        else{
                            directionDetail = "up";
                        }
                    }
                    timeBetweenSteps = 1.5f;
                }
            }

        }   
        else if(wholeNumbers == false){
            if(activationTime > 0){
                activationTime =- Time.deltaTime;
            }
            else{
                horizontal.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = activeColor;
                vertical.gameObject.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>().color = activeColor; 
                if(direction == "horizontal" && directionDetail == "right"){
                    if(horizontal.value < horizontal.maxValue){
                    horizontal.value = horizontal.value + 0.01f; 
                    }
                    else{
                        directionDetail = "left";
                    }
                }
                else if(direction == "horizontal" && directionDetail == "left"){
                    if(horizontal.value > horizontal.minValue){
                    horizontal.value = horizontal.value - 0.01f; 
                    }
                    else{
                        directionDetail = "right";
                    }
                }
                else if(direction == "vertical" && directionDetail == "up"){
                    if(vertical.value < vertical.maxValue){
                    vertical.value = vertical.value + 0.01f; 
                    }
                    else{
                        directionDetail = "down";
                    }
                }
                else if(direction == "vertical" && directionDetail == "down"){
                    if(vertical.value > vertical.minValue){
                    vertical.value = vertical.value - 0.01f; 
                    }
                    else{
                        directionDetail = "up";
                    }
                }
                activationTime = 3.0f;
            }
        }    
    }

    public void onEnable(){
        Start();
    }
}
