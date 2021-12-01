using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        public LeapHandPosition(string name, float tempX, float tempY, float tempZ){
            point = name; 
            x = tempX;
            y = tempY;
            z = tempZ;
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
    
    // Start is called before the first frame update
    void Start()
    {
        
        handpointTransform = handpoint.GetComponent<Transform>();

        print("point " +handpoint);
         
        filename = Application.dataPath + filename;
        print("Pfade " + filename);
    }

    // Update is called once per frame
    void Update()
    {
        /*float tempX = Random.Range(1.0f, 100.0f);
        float tempY = Random.Range(1.0f, 100.0f);
        float tempZ = Random.Range(1.0f, 100.0f);*/

        float tempX = handpointTransform.position.x; // Random.Range(1.0f, 100.0f);
        float tempY = handpointTransform.position.y;
        float tempZ = handpointTransform.position.z;

        LeapHandPosition newPosition = new LeapHandPosition (handpoint.ToString(), tempX, tempY, tempZ);
    
        System.Array.Resize(ref currentList.positions, currentList.positions.Length+1);
           
        currentList.positions[currentList.positions.Length-1] = newPosition;

        if(Input.GetKey("p"))
        {
            writeCSV();
        }
    }

    public void writeCSV()
    {
        print("Write CSV");
        if(currentList.positions.Length > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("Point ; X ; Y ; Z");
            tw.Close();

            tw = new StreamWriter(filename, true);
            for(int i = 0; i < currentList.positions.Length; i++)
            {
                tw.WriteLine(currentList.positions[i].point + ";" +
                             currentList.positions[i].x + ";"+
                             currentList.positions[i].y + ";"+
                             currentList.positions[i].z);
            }
            tw.Close();
        }
    }
}
