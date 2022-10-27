using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthManager : MonoBehaviour
{
    
    public Image[] Heart;

    Sprite fullHeart;
    Sprite emptyHeart;


    //플레이어에서 해당 변수 get
    public float currentPlayerHP { get; private set; }
    private float maxPlayerHP;

    private void Awake()
    {
        for (int i = 0; i < Heart.Length; i++)
        {
            Heart[i] = Heart[i].transform.GetChild(0).GetComponent<Image>();

        }
            maxPlayerHP = Heart.Length;

            currentPlayerHP = maxPlayerHP;
    }

    
    public void ExInit()
    {
        Heart = new Image[transform.childCount];

        for (int i = 0; i < Heart.Length; i++)
            Heart[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();

        maxPlayerHP = Heart.Length;

        currentPlayerHP = Mathf.Clamp(currentPlayerHP, 0, maxPlayerHP);

        for (int i = 0; i < maxPlayerHP; i++)
        {
            Heart[i].fillAmount = 0;

            if ((int)currentPlayerHP > i)
            {
                Heart[i].fillAmount = 1;
            }

            if ((int)currentPlayerHP == i)
                Heart[i].fillAmount = currentPlayerHP - (int)currentPlayerHP;
        }
    }

    public void SetHp(float val)
    {
        currentPlayerHP += val;

        currentPlayerHP = Mathf.Clamp(currentPlayerHP, 0, maxPlayerHP);

        for (int i = 0; i < maxPlayerHP; i++)
        {
            Heart[i].fillAmount = 0;
        }

        for (int i = 0; i < maxPlayerHP; i++)
        {
            Heart[i].fillAmount = 0;

            if ((int)currentPlayerHP > i)
            {
                Heart[i].fillAmount = 1;
            }

            if ((int)currentPlayerHP == i)
                Heart[i].fillAmount = currentPlayerHP - (int)currentPlayerHP;
        }
    }
}
