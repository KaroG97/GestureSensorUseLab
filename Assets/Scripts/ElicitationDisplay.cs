using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElicitationDisplay : MonoBehaviour
{

    public GameObject[] tasks;

    public GameObject Rotationcube;
    public GameObject Translationcube;

    public string activeTask;

    public string getActiveTask(){
        return activeTask;
    }

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("0"))
        {
            activeTask ="0";
            endAllTasks();
            beginTask(0);
            tasks[0].GetComponent<Task1>().enabled = true;
            tasks[0].GetComponent<Task1>().onEnable();
        }
        else if(Input.GetKeyDown("1"))
        {
            activeTask ="1";
            endAllTasks();
            beginTask(1);
            tasks[1].GetComponent<Task21>().enabled = true;
            tasks[1].GetComponent<Task21>().onEnable();
        }
        else if(Input.GetKeyDown("2"))
        {
            activeTask ="2";
            endAllTasks();
            beginTask(2);
            tasks[2].GetComponent<Task2>().enabled = true;
            tasks[2].GetComponent<Task2>().onEnable();
        }        
        else if(Input.GetKeyDown("3"))
        {
            activeTask ="3";
            endAllTasks();
            beginTask(3);
            tasks[3].GetComponent<Task3>().enabled = true;
            tasks[3].GetComponent<Task3>().onEnable();
        } 
        else if(Input.GetKeyDown("4"))
        {
            activeTask ="4";
            endAllTasks();
            beginTask(4);
            tasks[4].GetComponent<Task4>().enabled = true;
            tasks[4].GetComponent<Task4>().onEnable();
        }     
        else if(Input.GetKeyDown("5"))
        {
            activeTask ="5";
            endAllTasks();
            beginTask(5);
            tasks[5].GetComponent<Task5>().enabled = true;
            tasks[5].GetComponent<Task5>().onEnable();
        } 
        else if(Input.GetKeyDown("6"))
        {
            activeTask ="6";
            endAllTasks();
            beginTask(6);
            tasks[6].GetComponent<Task6>().enabled = true;
            tasks[6].GetComponent<Task6>().onEnable();
        }       
        else if(Input.GetKeyDown("7"))
        {
            activeTask ="7";
            endAllTasks();
            beginTask(7);
            tasks[7].GetComponent<Task7>().enabled = true;
            tasks[7].GetComponent<Task7>().onEnable();
        }   
        else if(Input.GetKeyDown("8"))
        {
            activeTask ="8";
            endAllTasks();
            beginTask(8);
            tasks[8].GetComponent<Task8>().enabled = true;
            tasks[8].GetComponent<Task8>().onEnable();
        }   
        else if(Input.GetKeyDown("9"))
        {
            activeTask ="9";
            endAllTasks();
            beginTask(9);
            tasks[9].GetComponent<Task9>().enabled = true;
            tasks[9].GetComponent<Task9>().onEnable();
        }     
    }

    void endAllTasks(){
        Rotationcube.SetActive(false);
        Translationcube.SetActive(false);
        for(int i = 0; i < tasks.Length; i++){
            tasks[i].SetActive(false); 
            if(i == 0){
                tasks[i].GetComponent<Task1>().enabled = false;
                //tasks[i].GetComponent<Task1>().onDisable();
            }
            if(i == 2){
                tasks[i].GetComponent<Task2>().enabled = false;                
            }
            if(i == 3){
                tasks[i].GetComponent<Task3>().enabled = false;
            }
        }       
    }

    void beginTask(int taskIndex){
        tasks[taskIndex].SetActive(true);
        for(int i = 0; i < GameObject.Find("sensorNeu").transform.childCount; i++){
            GameObject.Find("sensorNeu").transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            GameObject.Find("sensorNeu").transform.GetChild(i).GetComponent<OutlineHandler>().reloadInitialSensor();
        }
    }
}
