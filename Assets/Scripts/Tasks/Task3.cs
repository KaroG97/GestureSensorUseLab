using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task3 : MonoBehaviour
{

    public Image[] imageList;
    public int activeImageIndex;

    public float timeBetweenImages;

    // Start is called before the first frame update
    void Start()
    {
        activeImageIndex = 0;
        timeBetweenImages = 5.0f;
        imageList[activeImageIndex].transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBetweenImages > 0){
            timeBetweenImages -= Time.deltaTime;
            imageList[activeImageIndex].fillAmount = timeBetweenImages/4;
        }
        else{
            imageList[activeImageIndex].fillAmount = timeBetweenImages/4;
            if(activeImageIndex == imageList.Length-1){
                imageList[activeImageIndex].transform.parent.gameObject.SetActive(false);
                activeImageIndex = 0;
                imageList[activeImageIndex].transform.parent.gameObject.SetActive(true);
            } 
            // This part of code is only relevant, if different images should be shown                
            /*else{
                imageList[activeImageIndex].transform.parent.gameObject.SetActive(false);
                activeImageIndex++;
                imageList[activeImageIndex].transform.parent.gameObject.SetActive(true);
            }*/
            timeBetweenImages = 5.0f;
        }
    }

    public void onEnable(){
        Start();
    }
}
