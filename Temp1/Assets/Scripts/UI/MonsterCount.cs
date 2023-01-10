using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterCount : MonoBehaviour
{

    TextMeshProUGUI monsterCount;
    MainManager mainManager;

    
    

    private void Awake()
    {
        mainManager = MainManager.instance;
    }
    private void Start()
    {
        monsterCount = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        
    }

   
    // Update is called once per frame
    void Update()
    {
        if (mainManager.numOfStageEnemy > 0)
        {
        monsterCount.text = $"{mainManager.numOfStageEnemy-mainManager.numOfDieEnemy} / {mainManager.numOfStageEnemy}";

        }

        else
        {
            monsterCount.text = "Room Clear";
        }
    }
    
}
