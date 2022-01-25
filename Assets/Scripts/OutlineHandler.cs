using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{

    public AudioSource enter; 
    public AudioSource leave; 

    public bool greatOutline  = false;

    public GameObject wrist;
    public GameObject index;
    public GameObject thumb;
    public GameObject pinky;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkCollision(this.gameObject, wrist, index, thumb, pinky);
    }

    void checkCollision(GameObject area, GameObject wrist, GameObject index, GameObject thumb, GameObject pinky){
        Vector3[] pointsToCheck = new Vector3[4];

        Collider areaCollider = area.GetComponent<Collider>();

        pointsToCheck[0] = new Vector3(wrist.GetComponent<Transform>().position.x, wrist.GetComponent<Transform>().position.y, wrist.GetComponent<Transform>().position.z);
        pointsToCheck[1] = new Vector3(index.GetComponent<Transform>().position.x, index.GetComponent<Transform>().position.y, index.GetComponent<Transform>().position.z);
        pointsToCheck[2] = new Vector3(thumb.GetComponent<Transform>().position.x, thumb.GetComponent<Transform>().position.y, thumb.GetComponent<Transform>().position.z);
        pointsToCheck[3] = new Vector3(pinky.GetComponent<Transform>().position.x, pinky.GetComponent<Transform>().position.y, pinky.GetComponent<Transform>().position.z);
       
        for( int i = 0; i < 4; i++ ){
            if(areaCollider.bounds.Contains(pointsToCheck[i]))
            {
                if(greatOutline == false){
                    area.GetComponent<Renderer>().enabled = true;
                }
                else{
                    area.GetComponent<Renderer>().enabled = true;
                    leave.Play();
                }
                

            }
            else
            {
                if(greatOutline == false){
                    area.GetComponent<Renderer>().enabled = false;
                }
                else{
                    area.GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
}
