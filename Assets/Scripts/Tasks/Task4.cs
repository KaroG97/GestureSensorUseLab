using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task4 : MonoBehaviour
{
    public Image dummy; 
    public Image target;

    public Vector3 dummyOrigin;

    public float roundtime; 
    public int round;

    private RectTransform canvasRect;
    private Rect canvasRectWorld;

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        roundtime = 3.0f;

        canvasRect  = this.gameObject.GetComponent<RectTransform>();
        canvasRectWorld = GetWorldSapceRect(canvasRect);

        dummyOrigin =  dummy.transform.position;

        //Vector3[] canvasCorners = new Vector3[4];
        //canvasRect.GetWorldCorners(canvasCorners);
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
                print("enter");
                round ++;
                roundtime = 3.0f;
            }
            else if(round == 1){
                if(canvasRect.rect.Contains(dummyRect.localPosition)){
                    
                    if(dummyRectWorld.Overlaps(targetRectWorld)){
                        target.GetComponent<Image>().color =  Color.green;
                    }
                    else{
                        target.GetComponent<Image>().color =  Color.red;
                        dummy.transform.Translate(0.005f,0,0);
                        dummy.transform.Translate(0,0.005f,0);
                    }
                }
                else{                    
                    dummy.transform.Translate(dummyOrigin);//position.x = dummyOrigin[0];
                    //dummy.transform.Translate(dummyOrigin);//position.y = dummyOrigin[1];
                    //dummy.transform.Translate(dummyOrigin);//position.z = dummyOrigin[2];
                    roundtime = 3.0f;
                }
            }
        }
        else{
            roundtime = 3.0f;
        }
        /*}
        else{
            if(Input.GetKeyDown("return")){
                round ++;
                roundtime = 3.0f;
            }
            else if(round == 1){

            }*/
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
