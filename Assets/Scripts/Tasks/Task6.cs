using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task6 : MonoBehaviour
{

    public static string dimension;
    public GameObject cube;
    
    public static float roundtime; 
    public static int round;

    // Start is called before the first frame update
    void Start()
    {
        cube.SetActive(true);
        round = 1;
        roundtime = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown("return")){
            if(round == 2){
                round = 1;
            }
            else{
                round ++;
            }            
        }

        if(roundtime > 0){
            roundtime = roundtime -= Time.deltaTime;
        }
        else{
            print("kipp");
            if(round == 1){
                // + x
                cube.transform.Rotate(45.0f,0,0);
            }
            else if(round == 2){
                // - x
                cube.transform.Rotate(-45.0f,0,0);
            }
        
            /*if(round == 1){
                // + x
                cube.transform.Rotate(0.5f,0,0);
            }
            else if(round == 2){
                // - x
                cube.transform.Rotate(-0.5f,0,0);
            }
            else if(round == 3){
                // + y
                cube.transform.Rotate(0,0.5f,0);
            }
            else if(round == 4){
                // - y
                cube.transform.Rotate(0,-0.5f,0);
            }
            else if(round == 5){
                // + z
                cube.transform.Rotate(0,0,0.5f);
            }
            else if(round == 6){
                // - z
                cube.transform.Rotate(0,0,-0.5f);
            }*/
            round++;
            //cube.transform.rotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
            roundtime = 1.5f;
        }      
    }

    public void onEnable(){
        Start();
    }
}
