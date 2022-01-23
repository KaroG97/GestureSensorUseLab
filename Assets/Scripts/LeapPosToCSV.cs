using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class LeapPosToCSV : MonoBehaviour
{
    string filename = "/LeapHandPosition.csv";

    [System.Serializable]

    public class LeapHandPosition
    {
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

        public LeapHandPosition(Vector3 org1, Vector3 org2, Vector3 org3, Vector3 org4,
                                string name1, float tempX1, float tempY1, float tempZ1,
                                string name2, float tempX2, float tempY2, float tempZ2,
                                string name3, float tempX3, float tempY3, float tempZ3,
                                string name4, float tempX4, float tempY4, float tempZ4, string tmp){
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

    public GameObject sensorArea000;
    public GameObject sensorArea001;
    public GameObject sensorArea002;
    public GameObject sensorArea003;
    public GameObject sensorArea010;
    public GameObject sensorArea011;
    public GameObject sensorArea012;
    public GameObject sensorArea013;
    public GameObject sensorArea100;
    public GameObject sensorArea101;
    public GameObject sensorArea102;
    public GameObject sensorArea103;
    public GameObject sensorArea110;
    public GameObject sensorArea111;
    public GameObject sensorArea112;
    public GameObject sensorArea113;
    public GameObject sensorArea200;
    public GameObject sensorArea201;
    public GameObject sensorArea202;
    public GameObject sensorArea203;
    public GameObject sensorArea210;
    public GameObject sensorArea211;
    public GameObject sensorArea212;
    public GameObject sensorArea213;

      
    // Start is called before the first frame update
    void Start()
    {
       
        wristTransform = wrist.GetComponent<Transform>();
        indexTransform = index.GetComponent<Transform>();
        thumbTransform = thumb.GetComponent<Transform>();
        pinkyTransform = pinky.GetComponent<Transform>();

        filename = Application.dataPath + filename;

        InvokeRepeating("collectPosition", 0.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {    
        if(Input.GetKey("p"))
        {
            writeCSV();
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
                

        LeapHandPosition newPosition = new LeapHandPosition (origin1, origin2, origin3, origin4,
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
            tw.WriteLine("Timestamp; Point1 ; X-Absolute ; Y-Absolute ; Z-Absolute;X-Relative ; Y-Relative ; Z-Relative; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213;  Point2 ; X-Absolute ; Y-Absolute ; Z-Absolute;X-Relative ; Y-Relative ; Z-Relative; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213; Point3 ; X-Absolute ; Y-Absolute ; Z-Absolute;X-Relative ; Y-Relative ; Z-Relative; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213;Point4 ;X-Absolute ; Y-Absolute ; Z-Absolute;X-Relative ; Y-Relative ; Z-Relative; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213");
            tw.Close();

            tw = new StreamWriter(filename, true);

            string[] insideSensorArea000;
            string[] insideSensorArea001;
            string[] insideSensorArea002;
            string[] insideSensorArea003;
            string[] insideSensorArea010;
            string[] insideSensorArea011;
            string[] insideSensorArea012;
            string[] insideSensorArea013;
            string[] insideSensorArea100;
            string[] insideSensorArea101;
            string[] insideSensorArea102;
            string[] insideSensorArea103;
            string[] insideSensorArea110;
            string[] insideSensorArea111;
            string[] insideSensorArea112;
            string[] insideSensorArea113;
            string[] insideSensorArea200;
            string[] insideSensorArea201;
            string[] insideSensorArea202;
            string[] insideSensorArea203;
            string[] insideSensorArea210;
            string[] insideSensorArea211;
            string[] insideSensorArea212;
            string[] insideSensorArea213;

            for(int i = 0; i < currentList.positions.Length; i++)
            {
                //string insideArea = checkPointInsideArea(currentList.positions[i].x, currentList.positions[i].y, currentList.positions[i].z);
                
                insideSensorArea000 = checkPointInsideArea(sensorArea000, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea001 = checkPointInsideArea(sensorArea001, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea002 = checkPointInsideArea(sensorArea002,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea003 = checkPointInsideArea(sensorArea003,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea010 = checkPointInsideArea(sensorArea010, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea011 = checkPointInsideArea(sensorArea011, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea012 = checkPointInsideArea(sensorArea012, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea013 = checkPointInsideArea(sensorArea013,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea100 = checkPointInsideArea(sensorArea100, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea101 = checkPointInsideArea(sensorArea101,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea102 = checkPointInsideArea(sensorArea102,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea103 = checkPointInsideArea(sensorArea103, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea110 = checkPointInsideArea(sensorArea110, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea111 = checkPointInsideArea(sensorArea111,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea112 = checkPointInsideArea(sensorArea112, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea113 = checkPointInsideArea(sensorArea113,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea200 = checkPointInsideArea(sensorArea200,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea201 = checkPointInsideArea(sensorArea201,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea202 = checkPointInsideArea(sensorArea202,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea203 = checkPointInsideArea(sensorArea203, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea210 = checkPointInsideArea(sensorArea210, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea211 = checkPointInsideArea(sensorArea211, 
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea212 = checkPointInsideArea(sensorArea212,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);
                insideSensorArea213 = checkPointInsideArea(sensorArea213,
                                                           currentList.positions[i].absoluteX1, currentList.positions[i].absoluteY1, currentList.positions[i].absoluteZ1,
                                                           currentList.positions[i].absoluteX2, currentList.positions[i].absoluteY2, currentList.positions[i].absoluteZ2,
                                                           currentList.positions[i].absoluteX3, currentList.positions[i].absoluteY3, currentList.positions[i].absoluteZ3,
                                                           currentList.positions[i].absoluteX4, currentList.positions[i].absoluteY4, currentList.positions[i].absoluteZ4);

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
                
                tw.WriteLine(
                             currentList.positions[i].timestamp + ";" +
                             currentList.positions[i].point1 + ";" +
                             currentList.positions[i].absoluteX1 + ";"+
                             currentList.positions[i].absoluteY1 + ";"+
                             currentList.positions[i].absoluteZ1 + ";"+
                             relativeX1 + ";"+
                             relativeY1 + ";"+
                             relativeZ1 + ";"+
                             insideSensorArea000[0] + ";" +
                             insideSensorArea001[0] + ";" +
                             insideSensorArea002[0] + ";" +
                             insideSensorArea003[0] + ";" +
                             insideSensorArea010[0] + ";" +
                             insideSensorArea011[0] + ";" +
                             insideSensorArea012[0] + ";" +
                             insideSensorArea013[0] + ";" +
                             insideSensorArea100[0] + ";" +
                             insideSensorArea101[0] + ";" +
                             insideSensorArea102[0] + ";" +
                             insideSensorArea103[0] + ";" +
                             insideSensorArea110[0] + ";" +
                             insideSensorArea111[0] + ";" +
                             insideSensorArea112[0] + ";" +
                             insideSensorArea113[0] + ";" +
                             insideSensorArea200[0] + ";" +
                             insideSensorArea201[0] + ";" +
                             insideSensorArea202[0] + ";" +
                             insideSensorArea203[0] + ";" +
                             insideSensorArea210[0] + ";" +
                             insideSensorArea211[0] + ";" +
                             insideSensorArea212[0] + ";" +
                             insideSensorArea213[0] + ";" +
                             currentList.positions[i].point2 + ";" +
                             currentList.positions[i].absoluteX2 + ";"+
                             currentList.positions[i].absoluteY2 + ";"+
                             currentList.positions[i].absoluteZ2 + ";"+
                             relativeX2 + ";"+
                             relativeY2 + ";"+
                             relativeZ2 + ";"+
                             insideSensorArea000[1] + ";" +
                             insideSensorArea001[1] + ";" +
                             insideSensorArea002[1] + ";" +
                             insideSensorArea003[1] + ";" +
                             insideSensorArea010[1] + ";" +
                             insideSensorArea011[1] + ";" +
                             insideSensorArea012[1] + ";" +
                             insideSensorArea013[1] + ";" +
                             insideSensorArea100[1] + ";" +
                             insideSensorArea101[1] + ";" +
                             insideSensorArea102[1] + ";" +
                             insideSensorArea103[1] + ";" +
                             insideSensorArea110[1] + ";" +
                             insideSensorArea111[1] + ";" +
                             insideSensorArea112[1] + ";" +
                             insideSensorArea113[1] + ";" +
                             insideSensorArea200[1] + ";" +
                             insideSensorArea201[1] + ";" +
                             insideSensorArea202[1] + ";" +
                             insideSensorArea203[1] + ";" +
                             insideSensorArea210[1] + ";" +
                             insideSensorArea211[1] + ";" +
                             insideSensorArea212[1] + ";" +
                             insideSensorArea213[1] + ";" +
                             currentList.positions[i].point3 + ";" +
                             currentList.positions[i].absoluteX3 + ";"+
                             currentList.positions[i].absoluteY3 + ";"+
                             currentList.positions[i].absoluteZ3 + ";"+
                             relativeX3 + ";"+
                             relativeY3 + ";"+
                             relativeZ3 + ";"+
                             insideSensorArea000[2] + ";" +
                             insideSensorArea001[2] + ";" +
                             insideSensorArea002[2] + ";" +
                             insideSensorArea003[2] + ";" +
                             insideSensorArea010[2] + ";" +
                             insideSensorArea011[2] + ";" +
                             insideSensorArea012[2] + ";" +
                             insideSensorArea013[2] + ";" +
                             insideSensorArea100[2] + ";" +
                             insideSensorArea101[2] + ";" +
                             insideSensorArea102[2] + ";" +
                             insideSensorArea103[2] + ";" +
                             insideSensorArea110[2] + ";" +
                             insideSensorArea111[2] + ";" +
                             insideSensorArea112[2] + ";" +
                             insideSensorArea113[2] + ";" +
                             insideSensorArea200[2] + ";" +
                             insideSensorArea201[2] + ";" +
                             insideSensorArea202[2] + ";" +
                             insideSensorArea203[2] + ";" +
                             insideSensorArea210[2] + ";" +
                             insideSensorArea211[2] + ";" +
                             insideSensorArea212[2] + ";" +
                             insideSensorArea213[2] + ";" +
                             currentList.positions[i].point4 + ";" +
                             currentList.positions[i].absoluteX4 + ";"+
                             currentList.positions[i].absoluteY4 + ";"+
                             currentList.positions[i].absoluteZ4 + ";"+
                             relativeX4 + ";"+
                             relativeY4 + ";"+
                             relativeZ4 + ";"+
                             insideSensorArea000[3] + ";" +
                             insideSensorArea001[3] + ";" +
                             insideSensorArea002[3] + ";" +
                             insideSensorArea003[3] + ";" +
                             insideSensorArea010[3] + ";" +
                             insideSensorArea011[3] + ";" +
                             insideSensorArea012[3] + ";" +
                             insideSensorArea013[3] + ";" +
                             insideSensorArea100[3] + ";" +
                             insideSensorArea101[3] + ";" +
                             insideSensorArea102[3] + ";" +
                             insideSensorArea103[3] + ";" +
                             insideSensorArea110[3] + ";" +
                             insideSensorArea111[3] + ";" +
                             insideSensorArea112[3] + ";" +
                             insideSensorArea113[3] + ";" +
                             insideSensorArea200[3] + ";" +
                             insideSensorArea201[3] + ";" +
                             insideSensorArea202[3] + ";" +
                             insideSensorArea203[3] + ";" +
                             insideSensorArea210[3] + ";" +
                             insideSensorArea211[3] + ";" +
                             insideSensorArea212[3] + ";" +
                             insideSensorArea213[3]);
            }
            tw.Close();
        }
    }
}
