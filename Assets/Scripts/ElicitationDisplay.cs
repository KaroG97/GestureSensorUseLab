using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElicitationDisplay : MonoBehaviour
{

    public GameObject[] tasks;

    public string activeTask;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            activeTask ="1";
            endAllTasks();
            beginTask(1);
            tasks[0].GetComponent<Task1>().enabled = true;
            tasks[0].GetComponent<Task1>().onEnable();
        }
        else if(Input.GetKeyDown("2"))
        {
            activeTask ="2";
            endAllTasks();
            beginTask(2);
            tasks[1].GetComponent<Task2>().enabled = true;
            tasks[1].GetComponent<Task2>().onEnable();
        }
        else if(Input.GetKeyDown("3"))
        {
            activeTask ="3";
            endAllTasks();
            beginTask(3);
            tasks[2].GetComponent<Task3>().enabled = true;
            tasks[2].GetComponent<Task3>().onEnable();
        }        
    }

    void endAllTasks(){
        for(int i = 0; i < tasks.Length; i++){
            tasks[i].SetActive(false); 
            if(i == 0){
                tasks[i].GetComponent<Task1>().enabled = false;
                tasks[i].GetComponent<Task1>().onDisable();
            }
            if(i == 1){
                tasks[i].GetComponent<Task2>().enabled = false;
            }
            if(i == 2){
                tasks[i].GetComponent<Task3>().enabled = false;
            }
        }       
    }

    void beginTask(int taskIndex){
        tasks[taskIndex-1].SetActive(true);
        for(int i = 0; i < GameObject.Find("sensorNeu").transform.childCount; i++){
            GameObject.Find("sensorNeu").transform.GetChild(i).GetComponent<Renderer>().enabled = false;
            GameObject.Find("sensorNeu").transform.GetChild(i).GetComponent<OutlineHandler>().reloadInitialSensor();
        }
    }
}
