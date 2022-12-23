using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public TextMeshProUGUI[] text_time; // �ð��� ǥ���� text
    
    float currentTime; // �ð�.

    
    bool isPlaying=false;

    

    private void Start()
    {
        SetTimerOn();

        Player player = Player.instance;
        player.onDie += SetTimerOff;

        
    }
    private void Update() // �ٲ�� �ð��� text�� �ݿ� �� �� update �����ֱ�
    {
        if (isPlaying)
        {
            currentTime += 3*Time.deltaTime;
            //(int)time / 3600) //�ð�
            text_time[0].text = $"{((int)currentTime / 60 % 60)} min"; //��
            text_time[1].text = $"{((int)currentTime % 60)} sec"; //��
        }
    }
    
    public void SetTimerOn()
    { // �÷��� ����
       isPlaying = true;
        currentTime = 0.0f;
    }

    public void SetTimerOff()
    { // �÷��� ����

        isPlaying = false;
    }
}
