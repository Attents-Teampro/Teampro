using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Player와 healthpreferences(UI 변경 스크립트)간의 값을 get set하는 스크립트
//프로퍼티 개선 작업 필요
public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth instance;

    private EnemyHealthPreferences prefs;

    
    

    void Start()
    {
        instance = this;
        prefs = GetComponent<EnemyHealthPreferences>();
        
    }

    public float GetCurrentHealth()
    {
        
        return prefs.GetCurrentHealth();
    }

    public void SetCurrentHealth(float currentHealth)
    {
        
        prefs.SetCurrentHealth(currentHealth);
    }

    public float GetTotalHealth()
    {
        return prefs.GetTotalHealth();
    }

    public void SetTotalHealth(float amount)
    {
        
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
