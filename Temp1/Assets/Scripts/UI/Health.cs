using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health class is used as a bridge between health preferences and your game logic
/// </summary>
public class Health : MonoBehaviour
{
    public static Health instance;

    private HealthPreferences prefs;
    

    void Start()
    {
        instance = this;
        prefs = GetComponent<HealthPreferences>();
        
    }

    public float GetCurrentHealth()
    {
        return prefs.GetCurrentHealth();
    }

    public void SetCurrentHealth(int currentHealth)
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

    public void AddDamage(float damage)
    {
        prefs.AddDamage(damage);
    }

    public void AddHeal(float heal)
    {
        prefs.AddHeal(heal);
    }

 
    public void EnableInvincibility(bool enabled)
    {
        prefs.EnableInvincibility(enabled);
    }

    public void SetImagesAmount(int amount)
    {
        prefs.Init(amount);
    }

    public void AddImage()
    {
        prefs.AddImage();
    }

    public void RemoveImage()
    {
        prefs.RemoveImage();
    }

    public void SetFillType(Image.FillMethod type) {
        prefs.SetFillType(type);
    }

    public void Reset()
    {
        prefs.Reset();
    }
}
