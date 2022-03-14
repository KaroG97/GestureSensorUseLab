using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* Task 1:
* Objective: System activation
* Components: Middle Part Sensor, Monitor, Hand
* Preparation: Chose Left/Right Hand
* Description: Ziel dieser Aufgabe ist es, das System "zu aktivieren". Dazu haelt der Proband seine Hand in die Mitte des Sensors (den einzigen angzeigten Stein), bis sich die restlichen aufbauen und ein Button auf dem Monitor erscheint
* Monitor erscheint. Der Proband fuerht nun eine Geste zum Druecken des Buttons aus. damit ist die Aufgabe abgeschlossen.
*/

public class Task1 : MonoBehaviour
{

    public GameObject sensor;

    public bool initialisation;
    public float remainingInitTime;

    public float loadingDiff;
    public int round;

    public Image loadingCircle;

    string[] first = {"M10", "M12", "M18", "M4", "U11", "O11"}; 
    string[] second = {"M3", "M5", "M17", "M19", "M9", "M13", "O10", "O12", "U10", "U12"};
    string[] third = {"M1", "M2", "M6", "M7", "M8", "M14", "M15", "M16", "M20", "M21", "O3", "O4", "O5", "O9", "O13", "O17", "O17", "O18", "O19", "U3", "U4", "U5", "U9", "U13", "U17", "U17", "U18", "U19"};
    string[] forth ={"O1", "O2", "O6", "O7", "O8", "O14", "O15", "O16", "O20", "O21", "U1", "U2", "U6", "U7", "U8", "U14", "U15", "U16", "U20", "U21", "outline" };

    
    // Start is called before the first frame update
    void Start()
    {
        initialisation = true;
        remainingInitTime = 2;
        round = 1; 
        loadingDiff = 0.5f;
        for(int i = 0; i < sensor.transform.childCount; i++){
            if(sensor.transform.GetChild(i).name != "M11"){
                sensor.transform.GetChild(i).gameObject.SetActive(false);
            }
            else{
                sensor.transform.GetChild(i).gameObject.SetActive(true);
                sensor.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(initialisation == true){
            if(remainingInitTime > 0){
                loadingCircle.fillAmount = remainingInitTime/2;
                remainingInitTime -= Time.deltaTime;
            }
            else{
                loadingCircle.fillAmount = remainingInitTime/2;
                initialisation = false; 
                round ++;
            }
        }
        
        else{
            if(round == 2){
                if(loadingDiff > 0){
                    loadingDiff -= Time.deltaTime;
                }
                else{
                    for(int i = 0; i < sensor.transform.childCount; i++){       
                        for(int j = 0; j < first.Length; j++){
                            if(sensor.transform.GetChild(i).name == first[j]){
                                sensor.transform.GetChild(i).gameObject.SetActive(true);
                                sensor.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
                            }  
                        }                        
                    }
                    round ++;
                    loadingDiff = 0.5f;
                }
            }
            if(round == 3){
                if(loadingDiff > 0){
                    loadingDiff -= Time.deltaTime;
                }
                else{
                    for(int i = 0; i < sensor.transform.childCount; i++){       
                        for(int j = 0; j < second.Length; j++){
                            if(sensor.transform.GetChild(i).name == second[j]){
                                sensor.transform.GetChild(i).gameObject.SetActive(true);
                                sensor.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
                            }  
                        }                        
                    }
                    round ++;
                    loadingDiff = 0.5f;
                }
            }
            if(round == 4){
                if(loadingDiff > 0){
                    loadingDiff -= Time.deltaTime;
                }
                else{
                    for(int i = 0; i < sensor.transform.childCount; i++){       
                        for(int j = 0; j < third.Length; j++){
                            if(sensor.transform.GetChild(i).name == third[j]){
                                sensor.transform.GetChild(i).gameObject.SetActive(true);
                                sensor.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
                            }  
                        }                        
                    }
                    round ++;
                    loadingDiff = 0.5f;
                }
            }
            if(round == 5){
                if(loadingDiff > 0){
                    loadingDiff -= Time.deltaTime;
                }
                else{
                    for(int i = 0; i < sensor.transform.childCount; i++){       
                        for(int j = 0; j < forth.Length; j++){
                            if(sensor.transform.GetChild(i).name == forth[j]){
                                sensor.transform.GetChild(i).gameObject.SetActive(true);
                                sensor.transform.GetChild(i).GetComponent<Renderer>().enabled = true;
                            }  
                        }                        
                    }
                    round ++;
                    loadingDiff = 0.5f;
                }
            }
            /*if(round == 6){
                if(loadingDiff > 0){
                    loadingDiff -= Time.deltaTime;
                }
                else{
                    Start();
                }
            }*/
        }       
    }    
    
    public void onEnable(){
        Start();
    }

}
