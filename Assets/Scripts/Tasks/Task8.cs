using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task8 : MonoBehaviour
{

    // Define cutting planes

    public GameObject cuttingPlaneX;
    public GameObject cuttingPlaneY;
    public GameObject cuttingPlaneZ;

    private static Renderer cuttingPlaneXRenderer;
    private static Renderer cuttingPlaneYRenderer;
    private static Renderer cuttingPlaneZRenderer;

    private static string direction; 


    private int round;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        cuttingPlaneXRenderer = cuttingPlaneX.GetComponent<Renderer>();
        cuttingPlaneYRenderer = cuttingPlaneY.GetComponent<Renderer>();
        cuttingPlaneZRenderer = cuttingPlaneZ.GetComponent<Renderer>();
        round = 1;
        time = 10.0f;
        direction = "up";
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0){
            time -= Time.deltaTime;
            
            if(round == 1  && direction == "up"){
                cuttingPlaneXRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneX.transform.position.x < 292.229){
                    cuttingPlaneX.transform.Translate(0.002f,0,0);
                }
                else{
                    direction = "down";
                }               
            }
            else if(round == 1  && direction == "down"){
                cuttingPlaneXRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneX.transform.position.x > 291.8730){
                    cuttingPlaneX.transform.Translate(-0.002f,0,0);
                }
                else{
                    direction = "up";
                }               
            }
            if(round == 2  && direction == "up"){            
                cuttingPlaneYRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneY.transform.position.y < 292.48){  
                    cuttingPlaneY.transform.Translate(0,0.002f,0);
                }
                else{
                    direction = "down";
                }               
            }
            else if(round == 2  && direction == "down"){
                cuttingPlaneYRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneY.transform.position.y > 292.01){
                    cuttingPlaneY.transform.Translate(0,-0.002f,0);
                }
                else{
                    direction = "up";
                }               
            }
            if(round == 3  && direction == "up"){
                cuttingPlaneZRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneZ.transform.position.z < -89.345){
                    cuttingPlaneZ.transform.Translate(0,0,0.002f);
                }
                else{
                    direction = "down";
                }               
            }
            else if(round == 3  && direction == "down"){
                
                cuttingPlaneZRenderer.material.SetColor("_Color", Color.red);
                if(cuttingPlaneZ.transform.position.z > -89.688){                    
                    cuttingPlaneZ.transform.Translate(0,0,-0.002f);
                }
                else{
                    direction = "up";
                }               
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
            time = 10.0f;
        }
    }

    public void onEnable(){
        Start();
    }

    public void deselectAllPlanes(){
        print("Deselect all planes!");
        cuttingPlaneX.gameObject.SetActive(true);
        cuttingPlaneY.gameObject.SetActive(true);
        cuttingPlaneZ.gameObject.SetActive(true);
        cuttingPlaneXRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneYRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneZRenderer.material.SetColor("_Color", Color.white);
    }

}
