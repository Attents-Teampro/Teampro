using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterCount : MonoBehaviour
{

    TextMeshProUGUI monsterCount;

    int goldSum = 0;

    private void Start()
    {
        monsterCount = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        monsterCount.text = $"{((int)MainManager.instance.numOfDieEnemy)} EA";  //gold 읽기 전용 프로퍼티 생성 필요
    }
}
