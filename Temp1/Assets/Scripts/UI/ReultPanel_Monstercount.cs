using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReultPanel_Monstercount : MonoBehaviour
{
    TextMeshProUGUI resultMonsterCount_text;

    int totalKill = 0;


    private void Start()
    {

        resultMonsterCount_text = GetComponent<TextMeshProUGUI>();

        resultMonsterCount_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        resultMonsterCount_text.text = $"{MainManager.instance.numOfTotalKillEnemy}";
    }


}
