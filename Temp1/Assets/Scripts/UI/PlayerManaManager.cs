using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManaManager : MonoBehaviour
{
    
    public Image[] Mana;

    //플레이어에서 해당 변수 get
    public float currentPlayerMP { get; private set; }
    private float maxPlayerMP;

    private void Awake()
    {
        for (int i = 0; i < Mana.Length; i++)
        {
            Mana[i] = Mana[i].transform.GetChild(0).GetComponent<Image>();

        }
            maxPlayerMP = Mana.Length;

            currentPlayerMP = maxPlayerMP;
    }

    
    public void ExInit()
    {
        
        Mana = new Image[transform.childCount];

        for (int i = 0; i < Mana.Length; i++)
            Mana[i] = transform.GetChild(i).GetChild(0).GetComponent<Image>();

        maxPlayerMP = Mana.Length;

        currentPlayerMP = Mathf.Clamp(currentPlayerMP, 0, maxPlayerMP);

        for (int i = 0; i < maxPlayerMP; i++)
        {
            Mana[i].fillAmount = 0;

            if ((int)currentPlayerMP > i)
            {
                Mana[i].fillAmount = 1;
            }

            if ((int)currentPlayerMP == i)
                Mana[i].fillAmount = currentPlayerMP - (int)currentPlayerMP;
        }
    }

    public void SetMP(float val)
    {
        currentPlayerMP += val;

        currentPlayerMP = Mathf.Clamp(currentPlayerMP, 0, maxPlayerMP);

        for (int i = 0; i < maxPlayerMP; i++)
        {
            Mana[i].fillAmount = 0;
        }

        for (int i = 0; i < maxPlayerMP; i++)
        {
            Mana[i].fillAmount = 0;

            if ((int)currentPlayerMP > i)
            {
                Mana[i].fillAmount = 1;
            }

            if ((int)currentPlayerMP == i)
                Mana[i].fillAmount = currentPlayerMP - (int)currentPlayerMP;
        }
    }
}
