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
                
                Vector3[] v = new Vector3[4];
                dummyRect.GetWorldCorners(v);
                //print("V" + v[0].ToString("F4"));
                //print("V" + v[1].ToString("F4"));
                //print("V" + v[2].ToString("F4"));
                //print("V" + v[3].ToString("F4"));
                
                print("Dummy Position " + dummyRect.position.ToString("F4"));
                print("Target Position " + targetRect.position.ToString("F4"));
                if(dummyRect.position.x == targetRect.position.x &&
                   dummyRect.position.y == targetRect.position.y &&
                   dummyRect.position.z == targetRect.position.z ){
                    print("JA");
                }
                else{
                    print("NE");
                }

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
       }
    }

    Rect GetWorldSapceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        //print("Center" + r.position);
        //r.size = rt.TransformVector(r.size);        
        return r;
    }

}


