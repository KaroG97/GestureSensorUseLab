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
        public float x1;
        public float y1;
        public float z1;
        public string point2;
        public float x2;
        public float y2;
        public float z2;
        public string point3;
        public float x3;
        public float y3;
        public float z3;
        public string point4;
        public float x4;
        public float y4;
        public float z4;
        public string timestamp;

        public LeapHandPosition(string name1, float tempX1, float tempY1, float tempZ1,
                                string name2, float tempX2, float tempY2, float tempZ2,
                                string name3, float tempX3, float tempY3, float tempZ3,
                                string name4, float tempX4, float tempY4, float tempZ4, string tmp){
            point1 = name1; 
            x1 = tempX1;
            y1 = tempY1;
            z1 = tempZ1;
            point2 = name2; 
            x2 = tempX2;
            y2 = tempY2;
            z2 = tempZ2;
            point3 = name3; 
            x3 = tempX3;
            y3 = tempY3;
            z3 = tempZ3;
            point4 = name4; 
            x4 = tempX4;
            y4 = tempY4;
            z4 = tempZ4;
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

    public static Collider sensorArea000Collider;
    public static Collider sensorArea001Collider;
    public static Collider sensorArea002Collider;
    public static Collider sensorArea003Collider;
    public static Collider sensorArea010Collider;
    public static Collider sensorArea011Collider;
    public static Collider sensorArea012Collider;
    public static Collider sensorArea013Collider;
    public static Collider sensorArea100Collider;
    public static Collider sensorArea101Collider;
    public static Collider sensorArea102Collider;
    public static Collider sensorArea103Collider;
    public static Collider sensorArea110Collider;
    public static Collider sensorArea111Collider;
    public static Collider sensorArea112Collider;
    public static Collider sensorArea113Collider;
    public static Collider sensorArea200Collider;
    public static Collider sensorArea201Collider;
    public static Collider sensorArea202Collider;
    public static Collider sensorArea203Collider;
    public static Collider sensorArea210Collider;
    public static Collider sensorArea211Collider;
    public static Collider sensorArea212Collider;
    public static Collider sensorArea213Collider; 
    
    // Start is called before the first frame update
    void Start()
    {
        
        wristTransform = wrist.GetComponent<Transform>();
        indexTransform = index.GetComponent<Transform>();
        thumbTransform = thumb.GetComponent<Transform>();
        pinkyTransform = pinky.GetComponent<Transform>();

        filename = Application.dataPath + filename;

        //sensorAreaCollider = sensorArea.GetComponent<Collider>();

        sensorArea000Collider = sensorArea000.GetComponent<Collider>();
        sensorArea001Collider = sensorArea001.GetComponent<Collider>();
        sensorArea002Collider = sensorArea002.GetComponent<Collider>();
        sensorArea003Collider = sensorArea003.GetComponent<Collider>();
        sensorArea010Collider = sensorArea010.GetComponent<Collider>();
        sensorArea011Collider = sensorArea011.GetComponent<Collider>();
        sensorArea012Collider = sensorArea012.GetComponent<Collider>();
        sensorArea013Collider = sensorArea013.GetComponent<Collider>();
        sensorArea100Collider = sensorArea100.GetComponent<Collider>();
        sensorArea101Collider = sensorArea101.GetComponent<Collider>();
        sensorArea102Collider = sensorArea102.GetComponent<Collider>();
        sensorArea103Collider = sensorArea103.GetComponent<Collider>();
        sensorArea110Collider = sensorArea110.GetComponent<Collider>();
        sensorArea111Collider = sensorArea111.GetComponent<Collider>();
        sensorArea112Collider = sensorArea112.GetComponent<Collider>();
        sensorArea113Collider = sensorArea113.GetComponent<Collider>();
        sensorArea200Collider = sensorArea200.GetComponent<Collider>();
        sensorArea201Collider = sensorArea201.GetComponent<Collider>();
        sensorArea202Collider = sensorArea202.GetComponent<Collider>();
        sensorArea203Collider = sensorArea203.GetComponent<Collider>();
        sensorArea210Collider = sensorArea210.GetComponent<Collider>();
        sensorArea211Collider = sensorArea211.GetComponent<Collider>();
        sensorArea212Collider = sensorArea212.GetComponent<Collider>();
        sensorArea213Collider = sensorArea213.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        LeapHandPosition newPosition = new LeapHandPosition (wrist.ToString(), tempWristX, tempWristY, tempWristZ, 
                                                             index.ToString(), tempIndexX, tempIndexY, tempIndexZ,
                                                             thumb.ToString(), tempThumbX, tempThumbY, tempThumbZ,
                                                             pinky.ToString(), tempPinkyX, tempPinkyY, tempPinkyZ, timestamp);
    
        System.Array.Resize(ref currentList.positions, currentList.positions.Length+1);
           
        currentList.positions[currentList.positions.Length-1] = newPosition;

        if(Input.GetKey("p"))
        {
            writeCSV();
        }
    }

    public string[] checkPointInsideArea(Collider area, float x1, float y1, float z1,
                                                        float x2, float y2, float z2,
                                                        float x3, float y3, float z3,
                                                        float x4, float y4, float z4)
    {
        Vector3[] pointsToCheck = new Vector3[4];

        pointsToCheck[0] = new Vector3(x1, y1, z1);
        pointsToCheck[1] = new Vector3(x2, y2, z2);
        pointsToCheck[2] = new Vector3(x3, y3, z3);
        pointsToCheck[3] = new Vector3(x4, y4, z4);

        string[] result = new string[4];
        
        for( int i = 0; i < 4; i++ ){
            if(area.bounds.Contains(pointsToCheck[i]))
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
            tw.WriteLine("Timestamp; Point1 ; X ; Y ; Z; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213;  Point2 ; X ; Y ; Z; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213; Point3 ; X ; Y ; Z; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213;Point4 ; X ; Y ; Z; Inside000; Inside001; Inside002;Inside003;Inside010;Inside011;Inside012;Inside013;Inside100;Inside101;Inside102;Inside103;Inside110;Inside111;Inside112;Inside113;Inside200;Inside201;Inside202;Inside203;Inside210;Inside211;Inside212;Inside213");
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
                
                insideSensorArea000 = checkPointInsideArea(sensorArea000Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea001 = checkPointInsideArea(sensorArea001Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea002 = checkPointInsideArea(sensorArea002Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea003 = checkPointInsideArea(sensorArea003Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea010 = checkPointInsideArea(sensorArea010Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea011 = checkPointInsideArea(sensorArea011Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea012 = checkPointInsideArea(sensorArea012Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea013 = checkPointInsideArea(sensorArea013Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea100 = checkPointInsideArea(sensorArea100Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea101 = checkPointInsideArea(sensorArea101Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea102 = checkPointInsideArea(sensorArea102Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea103 = checkPointInsideArea(sensorArea103Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea110 = checkPointInsideArea(sensorArea110Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea111 = checkPointInsideArea(sensorArea111Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea112 = checkPointInsideArea(sensorArea112Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea113 = checkPointInsideArea(sensorArea113Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea200 = checkPointInsideArea(sensorArea200Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea201 = checkPointInsideArea(sensorArea201Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea202 = checkPointInsideArea(sensorArea202Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea203 = checkPointInsideArea(sensorArea203Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea210 = checkPointInsideArea(sensorArea210Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea211 = checkPointInsideArea(sensorArea211Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea212 = checkPointInsideArea(sensorArea212Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);
                insideSensorArea213 = checkPointInsideArea(sensorArea213Collider, 
                                                           currentList.positions[i].x1, currentList.positions[i].y1, currentList.positions[i].z1,
                                                           currentList.positions[i].x2, currentList.positions[i].y2, currentList.positions[i].z2,
                                                           currentList.positions[i].x3, currentList.positions[i].y3, currentList.positions[i].z3,
                                                           currentList.positions[i].x4, currentList.positions[i].y4, currentList.positions[i].z4);

                tw.WriteLine(
                             currentList.positions[i].timestamp + ";" +
                             currentList.positions[i].point1 + ";" +
                             currentList.positions[i].x1 + ";"+
                             currentList.positions[i].y1 + ";"+
                             currentList.positions[i].z1 + ";"+
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
                             currentList.positions[i].x2 + ";"+
                             currentList.positions[i].y2 + ";"+
                             currentList.positions[i].z2 + ";"+
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
                             currentList.positions[i].x3 + ";"+
                             currentList.positions[i].y3 + ";"+
                             currentList.positions[i].z3 + ";"+
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
                             currentList.positions[i].x4 + ";"+
                             currentList.positions[i].y4 + ";"+
                             currentList.positions[i].z4 + ";"+
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
