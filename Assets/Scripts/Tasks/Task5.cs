using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task5 : MonoBehaviour
{
    public Image dummy; 
    public Image target;

    public Vector3 dummyOrigin;

    public float roundtime; 
    public int round;

    private RectTransform canvasRect;
    private Rect canvasRectWorld;

    private int paused; 

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        roundtime = 3.0f;

        canvasRect  = this.gameObject.GetComponent<RectTransform>();
        canvasRectWorld = GetWorldSapceRect(canvasRect);

        dummyOrigin =  dummy.transform.position;
        paused = 0;
    }

    // Update is called once per frame
    void Update()
    {

        RectTransform dummyRect = dummy.GetComponent<RectTransform>();
        RectTransform targetRect = target.GetComponent<RectTransform>();
    
        Rect dummyRectWorld = GetWorldSapceRect(dummyRect);
        Rect targetRectWorld = GetWorldSapceRect(targetRect);

        Vector3[] dummyCorners = new Vector3[4];
        dummyRect.GetLocalCorners(dummyCorners);

        if(roundtime > 0){
            roundtime -= Time.deltaTime;
            if(Input.GetKeyDown("return")){               
                if(round == 3){
                    round = 1;
                }
                else{
                    round ++;
                }
                target.GetComponent<Image>().color =  Color.red;
                dummy.transform.position = dummyOrigin;
                roundtime = 3.0f;
            }
            else if(round == 1){
                if(canvasRect.rect.Contains(dummyRect.localPosition)){
                    
                    if(dummyRectWorld.Overlaps(targetRectWorld)){
                        target.GetComponent<Image>().color =  Color.green;
                        dummy.transform.position = target.transform.position;
                    }
                    else{
                        target.GetComponent<Image>().color =  Color.red;
                        dummy.transform.Translate(0.001f,0,0);
                        dummy.transform.Translate(0,0.001f,0);
                    }
                }
                else{                    
                    dummy.transform.position = dummyOrigin;
                    roundtime = 3.0f;
                }
            }
        
            else if(round == 2){
                if(canvasRect.rect.Contains(dummyRect.localPosition)){
                    
                    if(dummyRectWorld.Overlaps(targetRectWorld)){
                        target.GetComponent<Image>().color =  Color.green;
                        dummy.transform.position = target.transform.position;
                    }
                    else{
                        if(dummy.transform.position.x >= target.transform.position.x+0.01){
                            target.GetComponent<Image>().color =  Color.red;
                            dummy.transform.Translate(0.001f,0,0);                    
                        }
                        else{
                            target.GetComponent<Image>().color =  Color.red;
                            dummy.transform.Translate(0,0.001f,0);
                        }
                    }
                }
            }
            else if(round == 3){
                if(canvasRect.rect.Contains(dummyRect.localPosition)){
                    
                    if(dummyRectWorld.Overlaps(targetRectWorld)){
                        target.GetComponent<Image>().color =  Color.green;
                        dummy.transform.position = target.transform.position;
                    }
                    else{
                        if(paused == 0){
                            if(dummy.transform.position.x >= target.transform.position.x+0.065){
                                target.GetComponent<Image>().color =  Color.red;
                                dummy.transform.Translate(0.065f,0,0);           
                            }
                            else{
                                target.GetComponent<Image>().color =  Color.red;
                                dummy.transform.Translate(0,0.065f,0);
                            }
                            paused ++;
                        }
                        else if(paused == 20){
                            paused = 0;
                        }
                        else{
                            paused ++;
                        }
                    }
                }
            }
            else{                    
                dummy.transform.Translate(dummyOrigin);
                roundtime = 3.0f;
            }
        }
        else{
            roundtime = 3.0f;
        }
    }

    public void onEnable(){
        Start();
    }

    Rect GetWorldSapceRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;

        return new Rect(position.z - rectTransformHeight / 2f, position.y - rectTransformHeight / 2f, rectTransformHeight, rectTransformHeight);
    }

    
}
