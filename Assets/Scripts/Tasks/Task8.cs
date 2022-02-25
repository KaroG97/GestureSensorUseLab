using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task8 : MonoBehaviour
{

    public GameObject dummy;
    public static Vector3 originalPosition;
    public static Vector3 targetPisition;

    public static Collider targetCollider;

    public static float stepcount;

    public Vector3 difference;
    public Vector3 steps; 

    public static float time; 

    void Start()
    {
        dummy.SetActive(true);
        originalPosition = dummy.transform.position;
        print("Start " + originalPosition);
        targetCollider = this.gameObject.GetComponent<Collider>();
        targetPisition = this.gameObject.transform.position;
        difference = calculateDifference();

        stepcount = 100.0f;
        steps = difference/stepcount;  
        time = 3.0f;      

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("return")){ 
            dummy.transform.position = originalPosition;
            time = 3.0f;
        }

        if(time > 0){
            time -= Time.deltaTime;
        
            if(targetCollider.bounds.Contains(dummy.transform.position)){
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else{
                dummy.transform.Translate(steps[0], steps[1], steps[2]);
                this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
        else{
            dummy.transform.position = originalPosition;
            time = 3.0f;
        }
    }

    public void onEnable(){  
       Start();
    }

    Vector3 calculateDifference(){
        Vector3 diff = new Vector3();

        diff[0] = targetPisition[0] - dummy.transform.position[0];
        diff[1] = targetPisition[1] - dummy.transform.position[1];
        diff[2] = targetPisition[2] - dummy.transform.position[2];

        return diff;
    }

}
