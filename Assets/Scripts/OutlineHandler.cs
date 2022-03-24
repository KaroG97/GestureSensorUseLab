using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{

    public AudioSource enter; 
    public AudioSource leave; 

    public bool greatOutline  = false;

    private GameObject wrist;
    private GameObject index;
    private GameObject thumb;
    private GameObject pinky;

    public string currentTask;

    public ElicitationDisplay elicitationDisplay;


    // Start is called before the first frame update
    void Start()
    {
        elicitationDisplay = GameObject.Find("Monitor_Active").GetComponent<ElicitationDisplay>();
        reloadInitialSensor();
        wrist = transform.parent.gameObject.GetComponent<LeapPosToCSV>().wrist;
        index = transform.parent.gameObject.GetComponent<LeapPosToCSV>().index;
        thumb = transform.parent.gameObject.GetComponent<LeapPosToCSV>().thumb;
        pinky = transform.parent.gameObject.GetComponent<LeapPosToCSV>().pinky;         
    }


    // Update is called once per frame
    void Update()
    {
        checkCollision(this.gameObject, wrist, index, thumb, pinky);

        if(Input.GetKeyDown("r")){
            print("reload");
            reloadInitialSensor();
        }
    }

    void checkCollision(GameObject area, GameObject wrist, GameObject index, GameObject thumb, GameObject pinky){
        
        Vector3[] pointsToCheck = new Vector3[4];

        Collider areaCollider = area.GetComponent<Collider>();

        pointsToCheck[0] = new Vector3(wrist.GetComponent<Transform>().position.x, wrist.GetComponent<Transform>().position.y, wrist.GetComponent<Transform>().position.z);
        pointsToCheck[1] = new Vector3(index.GetComponent<Transform>().position.x, index.GetComponent<Transform>().position.y, index.GetComponent<Transform>().position.z);
        pointsToCheck[2] = new Vector3(thumb.GetComponent<Transform>().position.x, thumb.GetComponent<Transform>().position.y, thumb.GetComponent<Transform>().position.z);
        pointsToCheck[3] = new Vector3(pinky.GetComponent<Transform>().position.x, pinky.GetComponent<Transform>().position.y, pinky.GetComponent<Transform>().position.z);
       
        /*if(this.greatOutline == true){
            area.GetComponent<Renderer>().enabled = true;
        }
        else{*/
            for( int i = 0; i < 4; i++ ){
                if(areaCollider.bounds.Contains(pointsToCheck[i]))
                {
                    area.GetComponent<Renderer>().enabled = true;
                    if(leave != null){
                    leave.Play();
                }
                else
                {                
                    //if(currentTask != "0"){  
                        area.GetComponent<Renderer>().enabled = false;
                    //}     
                }
            }
            }
        //}
    }

    public void reloadInitialSensor(){
        currentTask = elicitationDisplay.activeTask;
        if(this.greatOutline == false){ 
                this.gameObject.SetActive(true); 
                this.gameObject.GetComponent<Renderer>().enabled = false;
        }
        else if(this.greatOutline == true && currentTask != "0"){
            this.gameObject.SetActive(true);
            this.gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
