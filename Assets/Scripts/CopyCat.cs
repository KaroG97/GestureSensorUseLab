using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCat : MonoBehaviour
{
    public GameObject Reference;
    //public GameObject liege;

    // Update is called once per frame
    void Update()
    {
        /*
        liege.transform.position = Reference.transform.position;
        liege.transform.rotation = Reference.transform.rotation;
        */
        transform.position = Reference.transform.position;
        transform.rotation = Reference.transform.rotation;
    }
    
    void LateUpdate()
    {
        transform.position = Reference.transform.position;
        transform.rotation = Reference.transform.rotation;
    }
    
}
