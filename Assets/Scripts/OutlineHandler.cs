using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHandler : MonoBehaviour
{

    public AudioClip enter; 
    public AudioClip leave; 

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
        checkCollision(this, wrist, index, thumb, pinky);
    }

    void checkCollision(GameObject collider, GameObject wrist, GameObject index, GameObject thumb, GameObject pinky){
        Vector3[] pointsToCheck = new Vector3[4];

        Collider areaCollider = area.GetComponent<Collider>();

        //pointsToCheck[0] = new Vector3(x1, y1, z1);
        //pointsToCheck[1] = new Vector3(x2, y2, z2);
        //pointsToCheck[2] = new Vector3(x3, y3, z3);
        //pointsToCheck[3] = new Vector3(x4, y4, z4);
       
        for( int i = 0; i < 4; i++ ){
            if(areaCollider.bounds.Contains(pointsToCheck[i]))
            {
                //var color = area.GetComponent<Renderer>().material.color;
                //area.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 0.2f);
                area.GetComponent<Renderer>().enabled = true;

            }
            else
            {
                //var color = area.GetComponent<Renderer>().material.color;
                area.GetComponent<Renderer>().enabled = false;
                //area.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 0.0f);
            }
        }
    }
}
