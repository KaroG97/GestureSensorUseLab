using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElicitationDisplay : MonoBehaviour
{

    public GameObject ui3D;
    public GameObject ui2D;
   

    public string activeUI;

    // Start is called before the first frame update
    void Start()
    {
        activeUI = "2D";
        ui3D.active = false;
        ui2D.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("2"))
        {
            activeUI = "2D";
            ui3D.active = false; 
            ui2D.active = true;
            print("Switched to 2D elicitation");
        }
        else if(Input.GetKey("3"))
        {
            activeUI = "3D";
            ui2D.active = false;
            ui3D.active = true;
            print("Switched to 3D elicitation");
        }

    }
}
