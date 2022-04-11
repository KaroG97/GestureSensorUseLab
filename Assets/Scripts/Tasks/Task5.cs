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
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(0.075f,0.075f, 0.0f);
                }
                else{
                    StartCoroutine(waiter());
                    direction = "smaller";
                }
                break;
            case "smaller":
                if(dummy.transform.localScale.x > min.x){
                    dummy.transform.localScale = dummy.transform.localScale + new Vector3(-0.075f,-0.075f, 0.0f);
                }
                else{
                    StartCoroutine(waiter());                    
                    direction = "bigger";
                }
                break;
            default: 
                break;
        }
    }

    public void wait(){
        print("Wait");
        float counter = 0;
        float waitTime = 1.0f;
        while(counter < waitTime){
            counter += Time.deltaTime;
        }
    }

    IEnumerator waiter(){
        yield return new WaitForSeconds(2f);
        print("jo");
    }

    public void onEnable(){
        Start();
    }
}
