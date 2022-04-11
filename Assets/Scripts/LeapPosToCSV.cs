using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class LeapPosToCSV : MonoBehaviour
{
    public string filename;

    string activeTask;

    public static GameObject monitor;

    public bool valid; 

    public ElicitationDisplay elicitationDisplay;

    [System.Serializable]

    public class LeapHandPosition
    {
        public bool valid;
        public string point1;
        public Vector3 origin1;
        public float absoluteX1;
        public float absoluteY1;
        public float absoluteZ1;
        public string point2;
        public Vector3 origin2;
        public float absoluteX2;
        public float absoluteY2;
        public float absoluteZ2;
        public string point3;
        public Vector3 origin3;
        public float absoluteX3;
        public float absoluteY3;
        public float absoluteZ3;
        public string point4;
        public Vector3 origin4;
        public float absoluteX4;
        public float absoluteY4;
        public float absoluteZ4;
        public string timestamp;

        public LeapHandPosition(bool val, Vector3 org1, Vector3 org2, Vector3 org3, Vector3 org4,
                                string name1, float tempX1, float tempY1, float tempZ1,
                                string name2, float tempX2, float tempY2, float tempZ2,
                                string name3, float tempX3, float tempY3, float tempZ3,
                                string name4, float tempX4, float tempY4, float tempZ4, string tmp){
            valid = val;
            point1 = name1; 
            origin1 = org1;
            absoluteX1 = tempX1;
            absoluteY1 = tempY1;
            absoluteZ1 = tempZ1;
            point2 = name2; 
            origin2 = org2;
            absoluteX2 = tempX2;
            absoluteY2 = tempY2;
            absoluteZ2 = tempZ2;
            point3 = name3; 
            origin3 = org3;
            absoluteX3 = tempX3;
            absoluteY3 = tempY3;
            absoluteZ3 = tempZ3;
            point4 = name4; 
            origin4 = org4;
            absoluteX4 = tempX4;
            absoluteY4 = tempY4;
            absoluteZ4 = tempZ4;
            timestamp = tmp;
        }
    }

    [System.Serializable]

    public class LeapHandPositionList
    {
        public LeapHandPosition[] positions;
    }

    public LeapHandPositionList currentList = new LeapHandPositionList();

    // Init handpoints

    public GameObject wrist;
    public GameObject index;
    public GameObject thumb;
    public GameObject pinky;

    public static Transform wristTransform;
    public static Transform indexTransform;
    public static Transform thumbTransform;
    public static Transform pinkyTransform;

    // Init Sensor Area to check collision

    public GameObject[] bottom;
    public GameObject[] middle;
    public GameObject[] top;
 
    // Start is called before the first frame update
    void Start()
    {

        
        elicitationDisplay = GameObject.Find("Monitor_Active").GetComponent<ElicitationDisplay>();
        activeTask = elicitationDisplay.getActiveTask();
        wristTransform = wrist.GetComponent<Transform>();
        indexTransform = index.GetComponent<Transform>();
        thumbTransform = thumb.GetComponent<Transform>();
        pinkyTransform = pinky.GetComponent<Transform>();

        filename = Application.dataPath + "/CSV/" + filename + ".csv";
        valid = false;
        InvokeRepeating("collectPosition", 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        activeTask = elicitationDisplay.getActiveTask();

        if(Input.GetKeyDown("g")){
            valid = true;
            print("Valid rec");
        }
        else if(Input.GetKeyDown("i")){
            valid = false;
            print("End valid rec");
        }
    }

    public void collectPosition(){


        float tempWristX = wristTransform.position.x; 
        float tempWristY = wristTransform.position.y;
        float tempWristZ = wristTransform.position.z;

        float tempThumbX = thumbTransform.position.x; 
        float tempThumbY = thumbTransform.position.y;
        float tempThumbZ = thumbTransform.position.z;

        float tempIndexX = indexTransform.position.x; 
        float tempIndexY = indexTransform.position.y;
        float tempIndexZ = indexTransform.position.z;

        float tempPinkyX = pinkyTransform.position.x; 
        float tempPinkyY = pinkyTransform.position.y;
        float tempPinkyZ = pinkyTransform.position.z;


        var culture = new CultureInfo("de-DE");
        string timestamp = System.DateTime.Now.ToString(culture);

        Vector3 origin1 = new Vector3();
        Vector3 origin2 = new Vector3();
        Vector3 origin3 = new Vector3();
        Vector3 origin4 = new Vector3();

        if(currentList.positions.Length >=1)
        {
            Vector3 tmp1 = new Vector3(currentList.positions[0].absoluteX1, currentList.positions[0].absoluteY1, currentList.positions[0].absoluteZ1);
            Vector3 tmp2 = new Vector3(currentList.positions[0].absoluteX2, currentList.positions[0].absoluteY2, currentList.positions[0].absoluteZ2);
            Vector3 tmp3 = new Vector3(currentList.positions[0].absoluteX3, currentList.positions[0].absoluteY3, currentList.positions[0].absoluteZ3);
            Vector3 tmp4 = new Vector3(currentList.positions[0].absoluteX4, currentList.positions[0].absoluteY4, currentList.positions[0].absoluteZ4);

            origin1 = tmp1;
            origin2 = tmp2;
            origin3 = tmp3;
            origin4 = tmp4;
        }
                

        LeapHandPosition newPosition = new LeapHandPosition (valid, origin1, origin2, origin3, origin4,
                                                             wrist.ToString(), tempWristX, tempWristY, tempWristZ, 
                                                             index.ToString(), tempIndexX, tempIndexY, tempIndexZ,
                                                             thumb.ToString(), tempThumbX, tempThumbY, tempThumbZ,
                                                             pinky.ToString(), tempPinkyX, tempPinkyY, tempPinkyZ, timestamp);
    
        System.Array.Resize(ref currentList.positions, currentList.positions.Length+1);
           
        currentList.positions[currentList.positions.Length-1] = newPosition;
    }

    public string[] checkPointInsideArea(GameObject area, float x1, float y1, float z1,
                                                        float x2, float y2, float z2,
                                                        float x3, float y3, float z3,
                                                        float x4, float y4, float z4)
    {
        Vector3[] pointsToCheck = new Vector3[4];

        Collider areaCollider = area.GetComponent<Collider>();

        pointsToCheck[0] = new Vector3(x1, y1, z1);
        pointsToCheck[1] = new Vector3(x2, y2, z2);
        pointsToCheck[2] = new Vector3(x3, y3, z3);
        pointsToCheck[3] = new Vector3(x4, y4, z4);

        string[] result = new string[4];
        
        for( int i = 0; i < 4; i++ ){
            if(areaCollider.bounds.Contains(pointsToCheck[i]))
            {
                result[i] = "true";

            }
            else
            {
                result[i] = "false";
            }
        }
        return result;
        
    }

    public void writeCSV()
    {
        print("Write CSV");
        if(currentList.positions.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            string header = "";
            header = concatHeader(4);
            tw.WriteLine(header);
            tw.Close();

            tw = new StreamWriter(filename, true);

            string[][] insideBottomArea = new string[bottom.Length][];
            string[][] insideMiddleArea = new string[middle.Length][];
            string[][] insideTopArea = new string[top.Length][];

            for(int i = 0; i < currentList.positions.Length; i++)
            {
                //assumption: same length in bottom, top and middle sensor area array
                for (int j = 0; j < bottom.Length; j++){
                    insideBottomArea[j] = checkPointInsideArea(bottom[j],
                                                                currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                                currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                                currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                                currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);

                    insideMiddleArea[j] = checkPointInsideArea(middle[j],
                                                                currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                                currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                                currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                                currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                    insideTopArea[j] = checkPointInsideArea(top[j],
                                                                currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                                currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                                currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                                currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                }
               

                float relativeX1 = currentList.positions[i].absoluteX1 - currentList.positions[i].origin1[0];
                float relativeY1 = currentList.positions[i].absoluteY1 - currentList.positions[i].origin1[1];
                float relativeZ1 = currentList.positions[i].absoluteZ1 - currentList.positions[i].origin1[2];

                float relativeX2 = currentList.positions[i].absoluteX2 - currentList.positions[i].origin2[0];
                float relativeY2 = currentList.positions[i].absoluteY2 - currentList.positions[i].origin2[1];
                float relativeZ2 = currentList.positions[i].absoluteZ2 - currentList.positions[i].origin2[2];

                float relativeX3 = currentList.positions[i].absoluteX3 - currentList.positions[i].origin3[0];
                float relativeY3 = currentList.positions[i].absoluteY3 - currentList.positions[i].origin3[1];
                float relativeZ3 = currentList.positions[i].absoluteZ3 - currentList.positions[i].origin3[2];
                
                float relativeX4 = currentList.positions[i].absoluteX4 - currentList.positions[i].origin4[0];
                float relativeY4 = currentList.positions[i].absoluteY4 - currentList.positions[i].origin4[1];
                float relativeZ4 = currentList.positions[i].absoluteZ4 - currentList.positions[i].origin4[2];
                
                string[] positionStringBottom = new string[4];
                string[] positionStringMiddle = new string[4];
                string[] positionStringTop = new string[4];

                for(int j = 0; j < positionStringBottom.Length; j ++){
                    positionStringBottom[j] = concatInsidePoisitionString(insideBottomArea, j);
                    positionStringMiddle[j] = concatInsidePoisitionString(insideMiddleArea, j);
                    positionStringTop[j] = concatInsidePoisitionString(insideTopArea, j);
                }

                tw.WriteLine(
                             currentList.positions[i].timestamp + ";" +
                             activeTask + ";" +
                             currentList.positions[i].valid + ";"+
                             currentList.positions[i].point1 + ";" +
                             currentList.positions[i].absoluteX1 + ";"+
                             currentList.positions[i].absoluteY1 + ";"+
                             currentList.positions[i].absoluteZ1 + ";"+
                             relativeX1 + ";"+
                             relativeY1 + ";"+
                             relativeZ1 + ";"+
                             positionStringBottom[0]  +
                             positionStringMiddle[0] +
                             positionStringTop[0] +                             
                             currentList.positions[i].point2 + ";" +
                             currentList.positions[i].absoluteX2 + ";"+
                             currentList.positions[i].absoluteY2 + ";"+
                             currentList.positions[i].absoluteZ2 + ";"+
                             relativeX2 + ";"+
                             relativeY2 + ";"+
                             relativeZ2 + ";"+
                             positionStringBottom[1] +
                             positionStringMiddle[1] +
                             positionStringTop[1] +
                             currentList.positions[i].point3 + ";" +
                             currentList.positions[i].absoluteX3 + ";"+
                             currentList.positions[i].absoluteY3 + ";"+
                             currentList.positions[i].absoluteZ3 + ";"+
                             relativeX3 + ";"+
                             relativeY3 + ";"+
                             relativeZ3 + ";"+
                             positionStringBottom[2] +
                             positionStringMiddle[2] +
                             positionStringTop[2] +
                             currentList.positions[i].point4 + ";" +
                             currentList.positions[i].absoluteX4 + ";"+
                             currentList.positions[i].absoluteY4 + ";"+
                             currentList.positions[i].absoluteZ4 + ";"+
                             relativeX4 + ";"+
                             relativeY4 + ";"+
                             relativeZ4 + ";"+                             
                             positionStringBottom[3] +
                             positionStringMiddle[3] +
                             positionStringTop[3]);
            }
            tw.Close();
        }

    }

    public string concatInsidePoisitionString(string[][] insidePositionString, int fingerIndex){
        int positionCountWidth = insidePositionString.GetLength(0);
       
        string positionString = ""; 

        for(int i = 0; i < positionCountWidth; i++){
            positionString += insidePositionString[i][fingerIndex]+";"; 
        }
        return positionString; 
    }

    public string concatHeader(int numPoints){
        string header = "";

        header += "Timestamp; ActiveTaskNumber; Valid Rep";

        for(int i = 1; i <= numPoints; i++){
            header += ";Point" + i + ";"+ "X-Absolute ; Y-Absolute ; Z-Absolute;X-Relative ; Y-Relative ; Z-Relative";
            for(int j = 1; j <= 63; j++){
                header += ";Inside"+j;
            }
        }

        return header;
    }

    void OnApplicationQuit(){
        writeCSV();
    }
}
