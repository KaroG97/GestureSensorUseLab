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
        public string point;
        public float x;
        public float y;
        public float z;
        public string timestamp;

        public LeapHandPosition(string name, float tempX, float tempY, float tempZ, string tmp){
            point = name; 
            x = tempX;
            y = tempY;
            z = tempZ;
            timestamp = tmp;
        }
    }

    [System.Serializable]

    public class LeapHandPositionList
    {
        public LeapHandPosition[] positions;
    }

    public LeapHandPositionList currentList = new LeapHandPositionList();

    public GameObject handpoint;

    public static Transform handpointTransform;

    // Init Sensor Area to check collision

    public GameObject sensorArea; 

    public static Collider sensorAreaCollider; 
    
    // Start is called before the first frame update
    void Start()
    {
        
        handpointTransform = handpoint.GetComponent<Transform>();
        filename = Application.dataPath + filename;
        sensorAreaCollider = sensorArea.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float tempX = handpointTransform.position.x; 
        float tempY = handpointTransform.position.y;
        float tempZ = handpointTransform.position.z;


        var culture = new CultureInfo("de-DE");
        string timestamp = System.DateTime.Now.ToString(culture);

        LeapHandPosition newPosition = new LeapHandPosition (handpoint.ToString(), tempX, tempY, tempZ, timestamp);
    
        System.Array.Resize(ref currentList.positions, currentList.positions.Length+1);
           
        currentList.positions[currentList.positions.Length-1] = newPosition;

        if(Input.GetKey("p"))
        {
            writeCSV();
        }
    }

    public string checkPointInsideArea(float x, float y, float z)
    {
        Vector3 pointToCheck = new Vector3(x, y, z);
        
        if(sensorAreaCollider.bounds.Contains(pointToCheck))
        {
            return "true";
        }
        else
        {
            return "false";
        }
    }

    public void writeCSV()
    {
        print("Write CSV");
        if(currentList.positions.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Point ; X ; Y ; Z; Inside Area; Timestamp");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for(int i = 0; i < currentList.positions.Length; i++)
            {
                string insideArea = checkPointInsideArea(currentList.positions[i].x, currentList.positions[i].y, currentList.positions[i].z);
                
                tw.WriteLine(currentList.positions[i].point + ";" +
                             currentList.positions[i].x + ";"+
                             currentList.positions[i].y + ";"+
                             currentList.positions[i].z + ";"+
                             insideArea + ";"+
                             currentList.positions[i].timestamp);
            }
            tw.Close();
        }
    }
}
