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

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;

    public string mode; 

    // Start is called before the first frame update
    void Start()
    {
        mode = "target";

        targetImage = target.GetComponent<Image>();
        dummyImage = target.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKey("b"))
       {    
           mode = "buttonList";
           print("Switched to button list mode!");
           button1.onClick.Invoke();
       } 
       else if(Input.GetKey("t"))
       {
           mode = "target";
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
            else if(Input.GetKey("return")){
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
                if(Input.GetKey("up")){

                }
                else if(Input.GetKey("down")){
        
                }
        break;
       }
    }


    // Function to transfer the image data from the canvas to world space
    Rect GetWorldSapceRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }

}


