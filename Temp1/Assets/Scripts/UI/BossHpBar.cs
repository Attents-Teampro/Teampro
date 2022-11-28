using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    GameObject parent;
    Slider slider;
    Enemy_Boss enemy_Boss;
    float eMaxHP;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        parent = transform.parent.gameObject;
        enemy_Boss = GetComponentInParent<Enemy_Boss>();
        eMaxHP = enemy_Boss.eHP;
    }

  

    private void Update()
    {
        slider.value = (float)enemy_Boss.eHP / (float)eMaxHP;
       
    }


    


}
