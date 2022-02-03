using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task2 : MonoBehaviour
{

    public Button[] buttonList;
    int[] order = {5, 1, 3, 7, 9, 2, 4, 6, 8};
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
            int tmp = order[activeButtonIndex] -1 ;
            buttonList[tmp].onClick.Invoke();
            if(activeButtonIndex == 8){
                activeButtonIndex = 0;
            }                
            else{
                activeButtonIndex++;
            }
            timeBetweenButtonPress = 1.5f;
        }        
    }
}
