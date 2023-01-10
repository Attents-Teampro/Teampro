using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class ResultPanel : MonoBehaviour
{
    //float resultTime;

    TextMeshProUGUI[] resultTime_texts;

    Timer timer;
    float resultTime;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        resultTime_texts = GetComponentsInChildren<TextMeshProUGUI>();

        
        resultTime_texts[0]=transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        resultTime_texts[1] = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        

        //resultTime_texts[0].text = $"{((int)timer.CurrentTime / 60 % 60)} min"; //분
        //resultTime_texts[1].text = $"{((int)timer.CurrentTime % 60)} sec"; //초
        
    }

    private void Update()
    {
        resultTime_texts[0].text = $"{((int)timer.CurrentTime / 60 % 60)} min"; //분
        resultTime_texts[1].text = $"{((int)timer.CurrentTime % 60)} sec"; //초
    }
}
