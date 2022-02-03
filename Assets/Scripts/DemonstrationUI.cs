using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonstrationUI : MonoBehaviour
{

    public GameObject target; 
    public GameObject dummy;

    private Image targetImage; 
    private Image dummyImage;

    private int activeButtonNumber;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    public static string mode; 

    public Button[] buttonList;
    public int activeButtonIndex;
    
    private static GameObject textDisplayMode;
    private static Text textMode;

    private static GameObject textDisplayDimension;
    private static Text textDimension;

    // Start is called before the first frame update
    void Start()
    {
        mode = "target";

        buttonList = new Button[5];
        buttonList[0] = button1;
        buttonList[1] = button2;
        buttonList[2] = button3;
        buttonList[3] = button4;
        buttonList[4] = button5;
        activeButtonIndex = 0;

        for(int i = 0; i < 5; i++){
               buttonList[i].gameObject.SetActive(false);
        }

        targetImage = target.GetComponent<Image>();
        dummyImage = target.GetComponent<Image>();

        // Fill output gameobjects

        textDisplayMode = GameObject.Find("DisplayMode");
        textMode = textDisplayMode.GetComponent<Text>();

        textDisplayDimension = GameObject.Find("DisplayDimension");
        textDimension = textDisplayDimension.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown("b"))
       {    
           mode = "buttonList";
           print("Switched to button list mode!");
           for(int i = 0; i < 5; i++){
               buttonList[i].gameObject.SetActive(true);
           }
           dummy.SetActive(false);
           target.SetActive(false);
           buttonList[activeButtonIndex].onClick.Invoke();
           textMode.text = mode;
       } 
       else if(Input.GetKeyDown("t"))
       {
           mode = "target";
           dummy.SetActive(true);
           target.SetActive(true);
           for(int i = 0; i < 5; i++){
               buttonList[i].gameObject.SetActive(false);
           }
           textMode.text = mode;

           print("Switched to target mode!");
       }
       
       switch(mode)
       {
        case "target":
            if(Input.GetKey("up")){                
                dummy.transform.Translate(0,0.01f,0);             
            }
            else if(Input.GetKey("down")){
                dummy.transform.Translate(0,-0.01f,0);
            }
            else if(Input.GetKey("right")){
                dummy.transform.Translate(0.01f,0,0);   
            }
            else if(Input.GetKey("left")){
                dummy.transform.Translate(-0.01f,0,0);   
            }
            else if(Input.GetKeyDown("return")){
                RectTransform dummyRect = dummy.GetComponent<RectTransform>();
                RectTransform targetRect = target.GetComponent<RectTransform>();
               
                Rect dummyRectWorld = GetWorldSapceRect(dummyRect);
                Rect targetRectWorld = GetWorldSapceRect(targetRect);

                if(dummyRectWorld.Overlaps(targetRectWorld)){
                    dummyImage.color =  Color.green;
                }
                else{
                    dummyImage.color =  Color.red;
                }    
            }
        break;       
        case "buttonList":
                if(Input.GetKeyDown("down")){
                    if(activeButtonIndex == 4){
                        activeButtonIndex = 0;
                    }
                    else{
                        activeButtonIndex ++;
                    }
                    buttonList[activeButtonIndex].onClick.Invoke();
                }
                else if(Input.GetKeyDown("up")){
                    if(activeButtonIndex == 0){
                        activeButtonIndex = 4;
                    }
                    else{
                        activeButtonIndex --;
                    }
                    buttonList[activeButtonIndex].onClick.Invoke();
                }
        break;
       }
    }


    // Function to transfer the image data from the canvas to world space
    Rect GetWorldSapceRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;

        return new Rect(position.z - rectTransformHeight / 2f, position.y - rectTransformHeight / 2f, rectTransformHeight, rectTransformHeight);
    }

    public static void Init(){
        mode = "target";
        textDimension.text = null;
        textMode.text = mode;
    }

}


