using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatShow : MonoBehaviour
{
    public float showTime = 0.3f;
    private float beginTime;

    private void Update()
    {
        if(beginTime + showTime < Time.time){
            gameObject.SetActive(false);
        }
    }

    public void Show(){
        gameObject.SetActive(true);
        beginTime = Time.time;
    }
}
