using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains all settings and health update logic
/// </summary>
public class HealthPreferences : MonoBehaviour
{
    public GameObject fullHeartsContainer;  //채워진 하트만 있는 콘테이너
    public GameObject emptyHeartsContainer; //빈 하트만 있는 콘테이너

    public Sprite fullHeartSprite;          //채워진 하트의 스프라이트
    public Sprite emptyHeartSprite;         //빈 하트의 스프라이트

    public int imagesAmount;                //만들어져있는 하트 갯수

    public float baseHealth = 100;          //현재 채력
    public float currentHealth;             //최대 체력

    private float valuePerImage;            //하트 이미지 하나가 자긴 hp 밸류 (최대 hp값 / 하트 갯수.ex)최대hp 100,하트 갯수 4일땐 25, 5면 20)
    private bool isInvincible = false;      //무적모드

    public Image.FillMethod fillMethod;     //이미지 채워지는 형태.필링 메소드

    //인스펙터창에서 가리기
    [HideInInspector]
    public Image.OriginHorizontal horizontalDirection;
    [HideInInspector]
    public Image.OriginVertical verticalDirection;
    [HideInInspector]
    public Image.Origin90 radial90Direction;
    [HideInInspector]
    public Image.Origin180 radial180Direction;
    [HideInInspector]
    public Image.Origin360 radial360Direction;


    /// <summary>
    /// Update health object with inspector changes while not playing 
    /// </summary>
    private void OnValidate()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, baseHealth);  //현재체력은 0에서 base 값 사이를 벗어나지 않는다

        if (gameObject.scene.IsValid()) //씬이 없다면?
        {
            if (!Application.isPlaying) //프로그램이 플레이중이라면
            {
                RemoveAll();  //모두 제거
#if UNITY_EDITOR
                for (int i = 0; i < imagesAmount; i++)
                    EditorApplication.delayCall += () => CreateImage(i);
#endif
            }
#if UNITY_EDITOR
            EditorApplication.delayCall += () => UpdateHealth();
#endif
        }
    }

    private void RemoveAll() 
    {
        if (Application.isPlaying)
        {
            foreach(Transform child in fullHeartsContainer.transform)
                DestroyImmediate(child.gameObject);

            foreach (Transform child in emptyHeartsContainer.transform)
                DestroyImmediate(child.gameObject);
        }
        else
        {
#if UNITY_EDITOR
            foreach (Transform child in fullHeartsContainer.transform)
                EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);

            foreach (Transform child in emptyHeartsContainer.transform)
                EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
#endif
        }
    }

    public void Init(int amount) //정도만큼 이미지를 만든다
    {
        imagesAmount = amount; 

        RemoveAll();

        for (int i = 0; i < imagesAmount; i++)
            CreateImage(i);
    }
    private void CreateImage(int index)
    {
        GameObject heartFull = new GameObject(); 
        heartFull.tag = "Heart Full";
        Image imgFull = heartFull.AddComponent<Image>();
        imgFull.sprite = fullHeartSprite;
        heartFull.GetComponent<RectTransform>().SetParent(fullHeartsContainer.transform);
        heartFull.transform.localScale = Vector3.one;

        imgFull.type = Image.Type.Filled;
        imgFull.fillMethod = fillMethod;
        switch (fillMethod)
        {
            case Image.FillMethod.Horizontal: imgFull.fillOrigin = (int)horizontalDirection; break;
            case Image.FillMethod.Vertical: imgFull.fillOrigin = (int)verticalDirection; break;
            case Image.FillMethod.Radial90: imgFull.fillOrigin = (int)radial90Direction; break;
            case Image.FillMethod.Radial180: imgFull.fillOrigin = (int)radial180Direction; break;
            case Image.FillMethod.Radial360: imgFull.fillOrigin = (int)radial360Direction; break;
        }

        valuePerImage = baseHealth / imagesAmount;

        if ((index + 1) * valuePerImage > currentHealth) //
        {
            float temp = (index + 1) * valuePerImage - currentHealth;
            float value = 1 - temp / valuePerImage;

            imgFull.fillAmount = value;
        }
        else
            imgFull.fillAmount = 1;

        if (index * valuePerImage >= currentHealth)
            imgFull.fillAmount = 0;


        GameObject heartEmpty = new GameObject();
        heartEmpty.tag = "Heart Empty";
        Image imgEmpty = heartEmpty.AddComponent<Image>();
        imgEmpty.sprite = emptyHeartSprite;
        heartEmpty.GetComponent<RectTransform>().SetParent(emptyHeartsContainer.transform);
        heartEmpty.transform.localScale = Vector3.one;
    }
    private void UpdateHealth()
    {
        valuePerImage = baseHealth / imagesAmount;

        for (int i = 0; i < imagesAmount; i++)
        {
            if ((i + 1) * valuePerImage > currentHealth)
            {
                float temp = (i + 1) * valuePerImage - currentHealth;
                float value = 1 - temp / valuePerImage;

                fullHeartsContainer.transform.GetChild(i).GetComponent<Image>().fillAmount = value;
            }
            else
                fullHeartsContainer.transform.GetChild(i).GetComponent<Image>().fillAmount = 1;

            if (i * valuePerImage >= currentHealth)
                fullHeartsContainer.transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
        }
    }

    public void SetCurrentHealth(float amount)
    {
        currentHealth = Mathf.Clamp(amount, 0, baseHealth);
        UpdateHealth();
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetTotalHealth()
    {
        return baseHealth;
    }

    /// <summary>
    /// Set base health amount
    /// </summary>
    public void SetTotalHealth(float amount) 
    {
        baseHealth = amount;
        UpdateHealth();
    }

    /// <summary>
    /// Decrease current health by amount
    /// </summary>
    /// <param name="amount"></param>
    public void AddDamage(float amount)
    {
        if (!isInvincible)
        {
            Debug.Log("HPref-add damage");
            currentHealth -= amount;

            if (currentHealth < 0)
                currentHealth = 0;

            UpdateHealth();
        }
    }

    public void AddHeal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > baseHealth)
            currentHealth = baseHealth;

        UpdateHealth();
    }

    public void AddImage()
    {
        imagesAmount += 1;
        Init(imagesAmount);
    }

    public void RemoveImage()
    {
        if (imagesAmount > 0)
        {
            imagesAmount -= 1;
            Init(imagesAmount);
        }
    }

    public void SetFillType(Image.FillMethod type)
    {
        fillMethod = type;
        Init(imagesAmount);
    }

    public void EnableInvincibility(bool enable)
    {
        if (enable)
            isInvincible = true;
        else
            isInvincible = false;
    }
    public void Reset()
    {
        imagesAmount = 3;
        baseHealth = 100;
        currentHealth = 100;
        fillMethod = Image.FillMethod.Horizontal;
        Init(imagesAmount);
    }
}
