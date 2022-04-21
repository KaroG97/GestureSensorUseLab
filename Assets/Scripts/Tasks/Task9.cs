using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task9 : MonoBehaviour
{

    public GameObject target;

    public GameObject dummy;
    public static Vector3 originalPosition;
    public static Vector3 targetPosition;

    public static Collider targetCollider;

    public static float stepcount;

    public Vector3 difference;
    public Vector3 steps; 

    public static float time; 

    void Start()
    {
        dummy.SetActive(true);
        //Store original dummy position to move dummy to origin if time is up
        originalPosition = dummy.transform.localPosition;
        //Find the position of the target Cube
        targetCollider = target.gameObject.GetComponent<Collider>();
        targetPosition = target.gameObject.transform.localPosition;
        //Calculate the initial distance between dummy and target
        difference = calculateDifference();
        //Define a stepsize using the distance divided by a stepcount
        stepcount = 50.0f;
        steps = difference/stepcount;
        //Define target time
        time = 5.0f; 
    }

    void Update()
    {
        //Recalculate distance between dummy and target and the resulting steps
        difference = calculateDifference();
        steps = difference/stepcount;

        //Restore original dummy position, if enter key is down
        if(Input.GetKeyDown("n")){ 
            target.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            dummy.transform.localPosition = originalPosition;
            time = 5.0f;
        }


        if(time > 0){
            time -= Time.deltaTime;
            if(time < 4.0f){
                if(targetCollider.bounds.Contains(dummy.transform.position)){
                    target.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                }
                else{
                    dummy.transform.localPosition = new Vector3(dummy.transform.localPosition[0]+steps[0], dummy.transform.localPosition[1]+steps[1], dummy.transform.localPosition[2]+steps[2]);
                    target.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
            }
        }
        //Restore original dummy position, if time is up and restart timer
        else{
            dummy.transform.localPosition = originalPosition;
            target.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            time = 5.0f;
        }
    }

    public void onEnable(){  
       Start();
    }

    Vector3 calculateDifference(){
        Vector3 diff = new Vector3();

        diff[0] = targetPosition[0] - dummy.transform.localPosition[0];
        diff[1] = targetPosition[1] - dummy.transform.localPosition[1];
        diff[2] = targetPosition[2] - dummy.transform.localPosition[2];
        return diff;
    }

}
