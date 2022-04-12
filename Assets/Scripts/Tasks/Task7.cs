using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task7 : MonoBehaviour
{

    // Define cutting planes

    public GameObject cuttingPlaneX;
    public GameObject cuttingPlaneY;
    public GameObject cuttingPlaneZ;

    private static Renderer cuttingPlaneXRenderer;
    private static Renderer cuttingPlaneYRenderer;
    private static Renderer cuttingPlaneZRenderer;

    private int round;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        cuttingPlaneXRenderer = cuttingPlaneX.GetComponent<Renderer>();
        cuttingPlaneYRenderer = cuttingPlaneY.GetComponent<Renderer>();
        cuttingPlaneZRenderer = cuttingPlaneZ.GetComponent<Renderer>();
        round = 1;
        time = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0){
            time -= Time.deltaTime;
            
            if(round == 1){
                cuttingPlaneZRenderer.material.SetColor("_Color", Color.red);       
            }
            else if(round == 2){            
                cuttingPlaneYRenderer.material.SetColor("_Color", Color.red);              
            }
            else if(round == 3){
                cuttingPlaneXRenderer.material.SetColor("_Color", Color.red);             
            }         
        }
        else{
            if(round < 3){
                deselectAllPlanes();
                round ++;
            }
            else{
                deselectAllPlanes();
                round = 1;
            }
            time = 1.5f;
        }
    }

    public void onEnable(){
        Start();
    }

    public void deselectAllPlanes(){
        cuttingPlaneX.gameObject.SetActive(true);
        cuttingPlaneY.gameObject.SetActive(true);
        cuttingPlaneZ.gameObject.SetActive(true);
        cuttingPlaneXRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneYRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneZRenderer.material.SetColor("_Color", Color.white);
    }

}
