using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI[] text_time; // 시간을 표시할 text
    
    float currentTime; // 시간.

    
    bool isPlaying=false;

    

    private void Start()
    {
        SetTimerOn();

        Player player = Player.instance;
        player.onDie += SetTimerOff;

        
    }
    private void Update() // 바뀌는 시간을 text에 반영 해 줄 update 생명주기
    {
        if (isPlaying)
        {
            currentTime += 3*Time.deltaTime;
            //(int)time / 3600) //시간
            text_time[0].text = $"{((int)currentTime / 60 % 60)} min"; //분
            text_time[1].text = $"{((int)currentTime % 60)} sec"; //초
        }
    }
    
    public void SetTimerOn()
    { // 플레이 시작
       isPlaying = true;
        currentTime = 0.0f;
    }

    public void SetTimerOff()
    { // 플레이 정지

        isPlaying = false;
    }
}
