using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
   public string mode = "r";
   public string dimension;

   
    private static GameObject textDisplayMode;
    private static Text textMode;

    private static GameObject textDisplayDimension;
    private static Text textDimension;

    void Start(){
        print("Welcome to the elicitation study demo. You can switch between modes using the keyboard");
        print("Press 'r' for rotation mode and 'x' / 'y' / 'z' for dimension");
        print("Press 's' for scaling mode and 'x' / 'y' / 'z' for dimension");
        print("Press 't' for scaling mode and 'x' / 'y' / 'z' for dimension");

        textDisplayMode = GameObject.Find("DisplayMode");
        textMode = textDisplayMode.GetComponent<Text>();

        textDisplayDimension = GameObject.Find("DisplayDimension");
        textDimension = textDisplayDimension.GetComponent<Text>();

    }

    void Update()
    {
        //Switch Modes
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

        //Switch Dimensions
        else if(Input.GetKey("x")){
            dimension = "x";
            print("Switched to dimension x!");
            textDimension.text = "x";
        }
        else if(Input.GetKey("y")){
            dimension = "y";
            print("Switched to dimension y!");
            textDimension.text = "y";
        }
        else if(Input.GetKey("z")){
            dimension = "z";
            print("Switched to dimension z!");
            textDimension.text = "z";
        }

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
            default:
                break;
        }
        

    }
}
