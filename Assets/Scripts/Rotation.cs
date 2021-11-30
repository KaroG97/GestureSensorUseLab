using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    public string mode;
    public string dimension;

   
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

    private static Renderer corner1Renderer;
    private static Renderer corner2Renderer;
    private static Renderer corner3Renderer;
    private static Renderer corner4Renderer;
    private static Renderer corner5Renderer;
    private static Renderer corner6Renderer;
    private static Renderer corner7Renderer;
    private static Renderer corner8Renderer;

    private string selectedCorner; 

    // Define cutting planes

    private static GameObject cuttingPlaneX;
    private static GameObject cuttingPlaneY;
    private static GameObject cuttingPlaneZ;

    private static Renderer cuttingPlaneXRenderer;
    private static Renderer cuttingPlaneYRenderer;
    private static Renderer cuttingPlaneZRenderer;

    private string selectedCuttingPlane;



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

        corner1 = GameObject.Find("corner1");
        corner2 = GameObject.Find("corner2");
        corner3 = GameObject.Find("corner3");
        corner4 = GameObject.Find("corner4");
        corner5 = GameObject.Find("corner5");
        corner6 = GameObject.Find("corner6");
        corner7 = GameObject.Find("corner7");
        corner8 = GameObject.Find("corner8");

        corner1Renderer = corner1.GetComponent<Renderer>();
        corner2Renderer = corner2.GetComponent<Renderer>();
        corner3Renderer = corner3.GetComponent<Renderer>();
        corner4Renderer = corner4.GetComponent<Renderer>();
        corner5Renderer = corner5.GetComponent<Renderer>();
        corner6Renderer = corner6.GetComponent<Renderer>();
        corner7Renderer = corner7.GetComponent<Renderer>();
        corner8Renderer = corner8.GetComponent<Renderer>();

        // Fill cutting plane gameobjects

        cuttingPlaneX = GameObject.Find("cuttingPlaneX");
        cuttingPlaneY = GameObject.Find("cuttingPlaneY");
        cuttingPlaneZ = GameObject.Find("cuttingPlaneZ");

        cuttingPlaneXRenderer = cuttingPlaneX.GetComponent<Renderer>();
        cuttingPlaneYRenderer = cuttingPlaneY.GetComponent<Renderer>();
        cuttingPlaneZRenderer = cuttingPlaneZ.GetComponent<Renderer>();
    }

    public void deselectAllCorners(){
        print("Deselect all corners!");
        corner1Renderer.material.SetColor("_Color", Color.white); 
        corner2Renderer.material.SetColor("_Color", Color.white);
        corner3Renderer.material.SetColor("_Color", Color.white);
        corner4Renderer.material.SetColor("_Color", Color.white);
        corner5Renderer.material.SetColor("_Color", Color.white);
        corner6Renderer.material.SetColor("_Color", Color.white);       
        corner7Renderer.material.SetColor("_Color", Color.white);
        corner8Renderer.material.SetColor("_Color", Color.white);
    }

    public void deselectAllPlanes(){
        print("Deselect all planes!");
        cuttingPlaneXRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneYRenderer.material.SetColor("_Color", Color.white);
        cuttingPlaneZRenderer.material.SetColor("_Color", Color.white);
    }

    void Update()
    {
        // Switch Modes
        if(Input.GetKey("r")){
            mode = "rotation";
            print("Switched to rotation mode!");
            textMode.text = "rotation";             
        }
        else if(Input.GetKey("s")){
            mode = "scaling";
            print("Switched to scaling mode!");
            textMode.text = "scaling";
        }
        else if(Input.GetKey("t")){
            mode = "translation";
            print("Switched to translation mode!");
            textMode.text = "translation";
        }

        // Switch Dimensions
        else if(Input.GetKey("x")){
            dimension = "x";
            print("Switched to dimension x!");
            textDimension.text = dimension;
        }
        else if(Input.GetKey("y")){
            dimension = "y";
            print("Switched to dimension y!");
            textDimension.text = dimension;
        }
        else if(Input.GetKey("z")){
            dimension = "z";
            print("Switched to dimension z!");
            textDimension.text = dimension;
        }

        // Select Corners

        else if(Input.GetKey("1")){
            deselectAllCorners();
            selectedCorner = "corner1";
            print("Selected Corner 1!");            
        }
        else if(Input.GetKey("2")){
            deselectAllCorners();
            selectedCorner = "corner2";
            print("Selected Corner 2!");            
        }
        else if(Input.GetKey("3")){
            deselectAllCorners();
            selectedCorner = "corner3";
            print("Selected Corner 3!");            
        }
        else if(Input.GetKey("4")){
            deselectAllCorners();
            selectedCorner = "corner4";
            print("Selected Corner 4!");            
        }
        else if(Input.GetKey("5")){
            deselectAllCorners();
            selectedCorner = "corner5";
            print("Selected Corner 5!");            
        }
        else if(Input.GetKey("6")){
            deselectAllCorners();
            selectedCorner = "corner6";
            print("Selected Corner 6!");            
        }
        else if(Input.GetKey("7")){
            deselectAllCorners();
            selectedCorner = "corner7";
            print("Selected Corner 7!");            
        }
        else if(Input.GetKey("8")){
            deselectAllCorners();
            selectedCorner = "corner8";
            print("Selected Corner 8!");            
        }

        // Select cutting planes

        else if(Input.GetKey("j")){
            deselectAllPlanes();
            mode = "cuttingPlane";
            dimension = "x";
            print("Switched to Cutting Plane Mode!");
            textMode.text = "cutting plane";
            textDimension.text = dimension;
            selectedCuttingPlane = "cuttingPlaneX";
            print("Selected Cutting Plane X");
        }
        else if(Input.GetKey("k")){
            deselectAllPlanes();
            mode = "cuttingPlane";
            dimension = "y";
            print("Switched to Cutting Plane Mode!");
            textMode.text = "cutting plane";
            textDimension.text = dimension;
            selectedCuttingPlane = "cuttingPlaneY";
            print("Selected Cutting Plane Y");
        }
        else if(Input.GetKey("l")){
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

        // Corner Selection

        switch(selectedCorner){            
            case "corner1":
                corner1Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner2":
                corner2Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner3":
                corner3Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner4":
                corner4Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner5":
                corner5Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner6":
                corner6Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner7":
                corner7Renderer.material.SetColor("_Color", Color.blue);
            break;
            case "corner8":
                corner8Renderer.material.SetColor("_Color", Color.blue);
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
            default:
                break;
        }
        

    }

   
}
