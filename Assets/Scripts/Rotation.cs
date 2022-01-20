using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public static string mode;
    public static string dimension;

   
    // Define gameobjects for text output

    private static GameObject textDisplayMode;
    private static Text textMode;

    private static GameObject textDisplayDimension;
    private static Text textDimension;

    // Define cube corners

    private static GameObject corner1;
    private static GameObject corner2;
    private static GameObject corner3;
    private static GameObject corner4;
    private static GameObject corner5;
    private static GameObject corner6;
    private static GameObject corner7;
    private static GameObject corner8;

    private static GameObject[] corners;

    private static Renderer corner1Renderer;
    private static Renderer corner2Renderer;
    private static Renderer corner3Renderer;
    private static Renderer corner4Renderer;
    private static Renderer corner5Renderer;
    private static Renderer corner6Renderer;
    private static Renderer corner7Renderer;
    private static Renderer corner8Renderer;

    //private string selectedCorner;
    private int activeCorner; 

    // Define cutting planes

    private static GameObject cuttingPlaneX;
    private static GameObject cuttingPlaneY;
    private static GameObject cuttingPlaneZ;

    private static Renderer cuttingPlaneXRenderer;
    private static Renderer cuttingPlaneYRenderer;
    private static Renderer cuttingPlaneZRenderer;

    private string selectedCuttingPlane;

    private static GameObject target;
    private static GameObject dummy;



    void Start(){
        print("Welcome to the elicitation study demo. You can switch between modes using the keyboard");
        print("Press 'r' for rotation mode and 'x' / 'y' / 'z' for dimension");
        print("Press 's' for scaling mode and 'x' / 'y' / 'z' for dimension");
        print("Press 't' for scaling mode and 'x' / 'y' / 'z' for dimension");
        print("Press '1-8' to select a corner");
        print("Press 'j' to select X cutting plane and up / down to move");
        print("Press 'k' to select Y cutting plane and up / down to move");
        print("Press 'l' to select Z cutting plane and up / down to move");

        // Fill output gameobjects

        textDisplayMode = GameObject.Find("DisplayMode");
        textMode = textDisplayMode.GetComponent<Text>();

        textDisplayDimension = GameObject.Find("DisplayDimension");
        textDimension = textDisplayDimension.GetComponent<Text>();

        // Fill corner gameobjects

        corners = new GameObject[8];
        activeCorner = 0;

        corners[0] = GameObject.Find("corner1");
        corners[1] = GameObject.Find("corner2");
        corners[2] = GameObject.Find("corner3");
        corners[3] = GameObject.Find("corner4");
        corners[4] = GameObject.Find("corner5");
        corners[5] = GameObject.Find("corner6");
        corners[6] = GameObject.Find("corner7");
        corners[7] = GameObject.Find("corner8");

        // Fill cutting plane gameobjects

        cuttingPlaneX = GameObject.Find("cuttingPlaneX");
        cuttingPlaneY = GameObject.Find("cuttingPlaneY");
        cuttingPlaneZ = GameObject.Find("cuttingPlaneZ");

        cuttingPlaneXRenderer = cuttingPlaneX.GetComponent<Renderer>();
        cuttingPlaneYRenderer = cuttingPlaneY.GetComponent<Renderer>();
        cuttingPlaneZRenderer = cuttingPlaneZ.GetComponent<Renderer>();
    }

    public void deselectAllCorners(){
        for(int i = 0; i < 8; i++){
            corners[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }

    public void deselectAllPlanes(){
        print("Deselect all planes!");
        cuttingPlaneXRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneYRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneZRenderer.material.SetColor("_Color", Color.white);
    }

    public static void Init(){
        mode = "rotation";
        dimension = "x";
        textDimension.text = dimension; 
        textMode.text = mode;
    }

    public static void checkPosition(){

        target = GameObject.Find("TargetCube");
        Collider targetCollider = target.GetComponent<Collider>();

        bool collision = false;

        for(int i = 0; i < 8; i++){
            Vector3 coordinates = corners[i].transform.position;
            if(targetCollider.bounds.Contains(coordinates)){
                collision = true; 
                break;               
            }
        }

        if(collision == true){
            target.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        else{
            target.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }        
    }

    void Update()
    {
        // Switch Modes
        if(Input.GetKeyDown("r")){
            mode = "rotation";
            print("Switched to rotation mode!");
            textMode.text = "rotation";             
        }
        else if(Input.GetKeyDown("s")){
            mode = "scaling";
            print("Switched to scaling mode!");
            textMode.text = "scaling";
        }
        else if(Input.GetKeyDown("t")){
            mode = "translation";
            print("Switched to translation mode!");
            textMode.text = "translation";
        }
        else if(Input.GetKeyDown("c")){
            mode = "corner";
            print("Switched to corner mode!");
            textMode.text = "corner";
        }

        // Switch Dimensions
        else if(Input.GetKeyDown("x")){
            dimension = "x";
            print("Switched to dimension x!");
            textDimension.text = dimension;
        }
        else if(Input.GetKeyDown("y")){
            dimension = "y";
            print("Switched to dimension y!");
            textDimension.text = dimension;
        }
        else if(Input.GetKeyDown("z")){
            dimension = "z";
            print("Switched to dimension z!");
            textDimension.text = dimension;
        }

        // Select cutting planes

        else if(Input.GetKeyDown("j")){
            deselectAllPlanes();
            mode = "cuttingPlane";
            dimension = "x";
            print("Switched to Cutting Plane Mode!");
            textMode.text = "cutting plane";
            textDimension.text = dimension;
            selectedCuttingPlane = "cuttingPlaneX";
            print("Selected Cutting Plane X");
        }
        else if(Input.GetKeyDown("k")){
            deselectAllPlanes();
            mode = "cuttingPlane";
            dimension = "y";
            print("Switched to Cutting Plane Mode!");
            textMode.text = "cutting plane";
            textDimension.text = dimension;
            selectedCuttingPlane = "cuttingPlaneY";
            print("Selected Cutting Plane Y");
        }
        else if(Input.GetKeyDown("l")){
            deselectAllPlanes();
            mode = "cuttingPlane";
            dimension = "z";
            print("Switched to Cutting Plane Mode!");
            textMode.text = "cutting plane";
            textDimension.text = dimension;
            selectedCuttingPlane = "cuttingPlaneZ";
            print("Selected Cutting Plane Z");
        }



        // Process Input

        // Cutting Plane Selection 

        switch(selectedCuttingPlane){
            case "cuttingPlaneX":
                cuttingPlaneXRenderer.material.SetColor("_Color", Color.red);
            break;
            case "cuttingPlaneY":
                cuttingPlaneYRenderer.material.SetColor("_Color", Color.red);
            break;
            case "cuttingPlaneZ":
                cuttingPlaneZRenderer.material.SetColor("_Color", Color.red);
            break;
            default:
            break;
        }

       

        // Geometric transformation

        switch(mode){
            case "rotation":
                if(Input.GetKey("up")){
                    if(dimension == "x"){
                     transform.Rotate(5,0,0);   
                    }
                    else if(dimension == "y"){
                        transform.Rotate(0,5,0);
                    }
                    else if(dimension == "z"){
                        transform.Rotate(0,0,5);
                    }
                }
                else if(Input.GetKey("down")){
                    if(dimension == "x"){
                        transform.Rotate(-5,0,0);   
                    }
                    else if(dimension == "y"){
                        transform.Rotate(0,-5,0);
                    }
                    else if(dimension == "z"){
                        transform.Rotate(0,0,-5);
                    }
                }
                break;
            case "scaling":
            if(Input.GetKey("up")){
                if(dimension == "x"){
                    transform.localScale += new Vector3(0.01f,0,0);   
                }
                else if(dimension == "y"){
                    transform.localScale += new Vector3(0,0.01f,0);
                }
                else if(dimension == "z"){
                    transform.localScale += new Vector3(0,0,0.01f);
                }
            }
            else if(Input.GetKey("down")){
                 if(dimension == "x"){
                    transform.localScale += new Vector3(-0.01f,0,0);   
                }
                else if(dimension == "y"){
                    transform.localScale += new Vector3(0,-0.01f,0);
                }
                else if(dimension == "z"){
                    transform.localScale += new Vector3(0,0,-0.01f);
                }
            }
            break;
            case "translation":
                if(Input.GetKey("up")){
                    if(dimension == "x"){
                     transform.Translate(0.01f,0,0);   
                    }
                    else if(dimension == "y"){
                        transform.Translate(0,0.01f,0);
                    }
                    else if(dimension == "z"){
                        transform.Translate(0,0,0.01f);
                    }
                }
                else if(Input.GetKey("down")){
                    if(dimension == "x"){
                        transform.Translate(-0.01f,0,0);   
                    }
                    else if(dimension == "y"){
                        transform.Translate(0,-0.01f,0);
                    }
                    else if(dimension == "z"){
                        transform.Translate(0,0,-0.01f);
                    }
                }
                else if(Input.GetKeyDown("return")){
                    checkPosition();
                }
                break;
            case "cuttingPlane":
                if(dimension == "x"){                     
                    if(Input.GetKey("up")){
                        cuttingPlaneX.transform.Translate(0.001f,0,0);
                    }
                    else if(Input.GetKey("down")){
                        cuttingPlaneX.transform.Translate(-0.001f,0,0);
                    }
                }
                else if(dimension == "y"){                     
                    if(Input.GetKey("up")){
                        cuttingPlaneY.transform.Translate(0,0.001f,0);
                    }
                    else if(Input.GetKey("down")){
                        cuttingPlaneY.transform.Translate(0,-0.001f,0);
                    }
                }
                else if(dimension == "z"){                     
                    if(Input.GetKey("up")){
                        cuttingPlaneZ.transform.Translate(0,0,0.001f);
                    }
                    else if(Input.GetKey("down")){
                        cuttingPlaneZ.transform.Translate(0,0, -0.001f);
                    }
                }
            break;
            case "corner":
                if(Input.GetKeyDown("down")){
                    deselectAllCorners();
                    if(activeCorner == 7){
                        activeCorner = 0;
                    }
                    else{
                        activeCorner ++;
                    }
                    //selectedCorner = corners[activeCorner];
                    corners[activeCorner].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                }
            else if(Input.GetKeyDown("up")){
                deselectAllCorners();
                if(activeCorner == 0){
                    activeCorner = 7;
                }
                else{
                    activeCorner --;
                }
                //selectedCorner = corners[activeCorner];
                corners[activeCorner].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
            break;
            default:
                break;
        }
        

    }

   
}
