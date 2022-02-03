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
        }
        else if(Input.GetKeyDown("2"))
        {
            activeTask ="2";
            endAllTasks();
            beginTask(2);
        }
        else if(Input.GetKeyDown("3"))
        {
            activeTask ="3";
            endAllTasks();
            beginTask(3);
        }
        /*else if(Input.GetKeyDown("3"))
        {
            activeUI = "3D";
            ui2D.active = false;
            ui3D.active = true;
            Rotation.Init();
            print("Switched to 3D elicitation");
        }*/

    }

    void endAllTasks(){
        for(int i = 0; i < tasks.Length; i++){
            tasks[i].SetActive(false); 
        }
    }

    void beginTask(int taskIndex){
        tasks[taskIndex-1].SetActive(true);
    }
}
