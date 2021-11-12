using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject dummy = GameObject.Find("DemonstrationCube");
    print("init script");
    print(dummy);
      //print(dummy.GetComponent<Rotation>.mode);
       //print(dummy.Rotation.dimension); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
