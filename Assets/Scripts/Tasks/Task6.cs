using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task6 : MonoBehaviour
{

    public Image dummy;

    public static string direction;

    public static Vector3 max;
    public static Vector3 min;



    // Start is called before the first frame update
    void Start()
    {
        direction = "bigger";

        min = dummy.transform.localScale;
        max = new Vector3(15.0f, 15.0f, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
        switch (direction){
            case "bigger": 
                if(dummy.transform.localScale.x < max.x){
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(0.1f,0.1f, 0.0f);
                }
                else{
                    direction = "smaller";
                }
                break;
            case "smaller":
                if(dummy.transform.localScale.x > min.x){
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(-0.1f,-0.1f, 0.0f);
                }
                else{
                    direction = "bigger";
                }
                break;
            default: 
                break;
        }
    }

    public void onEnable(){
        Start();
    }
}
