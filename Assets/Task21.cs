using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task21 : MonoBehaviour
{
    public Button[] buttonList;
    public int activeButtonIndex;

    public float timeBetweenButtonPress; 

    // Start is called before the first frame update
    void Start()
    {
       activeButtonIndex = 0;
       timeBetweenButtonPress = 1.5f;        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBetweenButtonPress > 0){
            timeBetweenButtonPress -= Time.deltaTime;
        }
        else{
            if(activeButtonIndex == 1){
                activeButtonIndex = 0;
            }                
            else{
                activeButtonIndex++;
            }
            buttonList[activeButtonIndex].onClick.Invoke();
            timeBetweenButtonPress = 1.5f;
        }        
    }

    public void onEnable(){
        Start();
    }
}
