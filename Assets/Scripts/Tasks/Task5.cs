using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task5 : MonoBehaviour
{

    public Image dummy;

    public static string direction;

    public static Vector3 max;
    public static Vector3 min;

    public float resizeTime;


    // Start is called before the first frame update
    void Start()
    {
        direction = "bigger";

        min = dummy.transform.localScale;
        max = new Vector3(15.0f, 15.0f, 0.0f);

        resizeTime = 5.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if(resizeTime > 0){
            resizeTime -= Time.deltaTime;
            if(direction == "bigger"){
                if(dummy.transform.localScale.x < max.x){
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(0.075f,0.075f, 0.0f);
                }
            }
            else if(direction == "smaller"){
                if(dummy.transform.localScale.x > min.x){
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(-0.075f,-0.075f, 0.0f);
                }               
            }
        }
        else{
            print("biggest");
            if(direction == "bigger"){
                direction = "smaller";
            }
            else if(direction == "smaller"){
                direction = "bigger";
            }
            resizeTime = 5.0f;
        }
    }

    public void onEnable(){
        Start();
    }
}
