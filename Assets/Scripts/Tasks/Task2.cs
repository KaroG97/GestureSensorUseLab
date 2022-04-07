using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task2 : MonoBehaviour
{

    public Button[] buttonList;
    public int[] order;
    //int[] order = {5, 1, 3, 7, 9, 2, 4, 6, 8};
    public int activeButtonIndex;

    public float timeBetweenButtonPress; 

    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
       activeButtonIndex = 0;
       timeBetweenButtonPress = 1.5f;  
       numberToScreen();
    }

    void numberToScreen(){
        textMesh.text = string.Empty;
        for(int i = 0; i < order.Length; i++){
           int tmp = order[i];
           textMesh.text = textMesh.text + tmp.ToString();
       }
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
            if(activeButtonIndex == order.Length-1){
                activeButtonIndex = 0;
            }                
            else{
                activeButtonIndex++;
            }
            timeBetweenButtonPress = 1.5f;
        }        
    }

    public void onEnable(){
        Start();
    }
}
