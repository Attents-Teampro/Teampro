using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldResult : MonoBehaviour
{

    TextMeshProUGUI goldResult;

    

    private void Start()
    {
        goldResult = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        goldResult.text = $"{((int)MainManager.instance.gold)} Gold";  //gold �б� ���� ������Ƽ ���� �ʿ�
    }
}
