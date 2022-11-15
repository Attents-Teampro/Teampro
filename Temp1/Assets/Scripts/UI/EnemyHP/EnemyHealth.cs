using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Player와 healthpreferences(UI 변경 스크립트)간의 값을 get set하는 스크립트
//프로퍼티 개선 작업 필요
public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth instance;

    private EnemyHealthPreferences prefs;

    public int maxHP;
    public int currentHP;

    Enemy_Orc orc;
    Enemy_Mage mage;
    Enemy_Shell shell;
    Enemy_Skelleton skeleton;
    GameObject parent;

    int parentHP;
    void Start()
    {
        parent = transform.parent.transform.parent.gameObject;
        if(parent.name == "Orc")
        {
        orc = parent.GetComponent<Enemy_Orc>();
        maxHP = orc.maxHP;
            currentHP = orc.currentHP;

        }
        else if(parent.name == "Mage")
        {
            mage = parent.GetComponent<Enemy_Mage>();
            maxHP = mage.maxHP;
            currentHP = mage.currentHP;
            //Debug.Log($"{parent.name}의 현재 HP : {currentHP}");
        }
        else if(parent.name == "Shell")
        {
            shell = parent.GetComponent<Enemy_Shell>();
            maxHP = shell.maxHP;
            currentHP = shell.currentHP;
        }
        else if(parent.name == "Skeleton")
        {
            skeleton = parent.GetComponent<Enemy_Skelleton>();
            maxHP = skeleton.maxHP;
            currentHP = skeleton.currentHP;
        }
    
        prefs = GetComponent<EnemyHealthPreferences>();
        //EnemyHealth.instance.SetCurrentHealth(currentHP);
        //EnemyHealth.instance.SetTotalHealth(maxHP);
        instance = this;
        
    }

    private void LateUpdate()
    {
        if (parent.name == "Orc")
        {
            currentHP = orc.currentHP;

        }
        else if (parent.name == "Mage")
        {
            currentHP = mage.currentHP;
        }
        else if (parent.name == "Shell")
        {
            currentHP = shell.currentHP;
        }
        else if (parent.name == "Skeleton")
        {
            currentHP = skeleton.currentHP;
        }
        Debug.Log($"{parent.name}의 현재 HP{currentHP}");
    }

    public float GetCurrentHealth()
    {
        return prefs.GetCurrentHealth();
    }

    public void SetCurrentHealth(float currentHealth)
    {
        Debug.Log($"SetCurrentHealth {currentHealth}");
        prefs.SetCurrentHealth(currentHealth);
    }

    public float GetTotalHealth()
    {
        return prefs.GetTotalHealth();
    }

    public void SetTotalHealth(float amount)
    {
        Debug.Log($"SetTotalHealth {amount}");
        prefs.SetTotalHealth(amount);
    }

    
    //misc
    //public void AddDamage(float damage)
    //{    
    //    prefs.AddDamage(damage);
    //}

    //public void AddHeal(float heal)
    //{
    //    prefs.AddHeal(heal);
    //}

    //public void SetImagesAmount(int amount)
    //{
    //    prefs.Init(amount);
    //}

    //public void AddImage()
    //{
    //    prefs.AddImage();
    //}

    //public void RemoveImage()
    //{
    //    prefs.RemoveImage();
    //}

    //public void SetFillType(Image.FillMethod type) {
    //    prefs.SetFillType(type);
    //}

    //public void Reset()
    //{
    //    prefs.Reset();
    //}
}
