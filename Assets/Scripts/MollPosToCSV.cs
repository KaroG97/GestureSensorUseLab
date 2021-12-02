using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;


public class MollPosToCSV : MonoBehaviour
{
    string filename = "/MollHandPosition.csv";

    [System.Serializable]

    public class MollHandPosition
    {
        public string point;
        public float x;
        public float y;
        public float z;

        public MollHandPosition(string name, float tempX, float tempY, float tempZ){
            point = name; 
            x = tempX;
            y = tempY;
            z = tempZ;
        }
    }

    [System.Serializable]

    public class MollHandPositionList
    {
        public MollHandPosition[] positions;
    }

    public MollHandPositionList currentList = new MollHandPositionList();

    public GameObject handpoint;

    public static Transform handpointTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
        handpointTransform = handpoint.GetComponent<Transform>();
        filename = Application.dataPath + filename;
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

        MollHandPosition newPosition = new MollHandPosition (handpoint.ToString(), tempX, tempY, tempZ);
    
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
            tw.WriteLine("Point ; X ; Y ; Z; Timestamp");
            tw.Close();

            tw = new StreamWriter(filename, true);

            var culture = new CultureInfo("de-DE");

            for(int i = 0; i < currentList.positions.Length; i++)
            {
                tw.WriteLine(currentList.positions[i].point + ";" +
                             currentList.positions[i].x + ";"+
                             currentList.positions[i].y + ";"+
                             currentList.positions[i].z + ";"+
                             System.DateTime.Now.ToString(culture));
            }
            tw.Close();
        }
    }
}
