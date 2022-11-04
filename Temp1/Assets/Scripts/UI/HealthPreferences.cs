using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class HealthPreferences : MonoBehaviour
{
    public GameObject fullHeartsContainer;  //채워진 하트만 있는 콘테이너
    public GameObject emptyHeartsContainer; //빈 하트만 있는 콘테이너

    public Sprite fullHeartSprite;          //채워진 하트의 스프라이트
    public Sprite emptyHeartSprite;         //빈 하트의 스프라이트

    public int imagesAmount;                //만들어져있는 하트 갯수

    public float maxHealth;               //최대 체력
    public float currentHealth;          //현재 체력

    private float valuePerImage;            //하트 이미지 하나가 자긴 hp 밸류 (최대 hp값 / 하트 갯수.ex)최대hp 100,하트 갯수 4일땐 25, 5면 20)

    Player player;
    public Image.FillMethod fillMethod;     //이미지 채워지는 방식

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

    


    private void Start()
    {

        player = Player.instance;
        maxHealth = player.pMaxHp;
        currentHealth = player.pHP;


    }

    private void OnValidate()
    {
        
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  //현재체력은 0에서 base 값 사이를 벗어나지 않는다

        if (gameObject.scene.IsValid()) //씬이 없다면?
        {
            if (!Application.isPlaying) //프로그램이 플레이중이면
            {
                RemoveAll();  //모두 제거
#if UNITY_EDITOR
                for (int i = 0; i < imagesAmount; i++)
                    EditorApplication.delayCall += () => CreateImage(i); //하트 이미지를 생성한다
#endif
            }
#if UNITY_EDITOR
            EditorApplication.delayCall += () => UpdateHealth();
#endif
        }
    }

    


    //플레이로 전환했을때 
    private void RemoveAll() 
    {
        if (Application.isPlaying)
        {
            foreach(Transform child in fullHeartsContainer.transform)
                Destroy(child.gameObject);

            foreach (Transform child in emptyHeartsContainer.transform)
                Destroy(child.gameObject);
        }
        else
        {
#if UNITY_EDITOR
            foreach (Transform child in fullHeartsContainer.transform)
                //아래 코드가 게임 종료 후 에러가 생성됩니다. by 손동욱 11.04
                EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
            ///에러 메세지 
            ///InvalidOperationException: Destroying a GameObject inside a Prefab instance is not allowed.
            ///UnityEngine.Object.DestroyImmediate(UnityEngine.Object obj)(at < 823fb226a3f9439cb41fdcb61f9c86a1 >:0)
            ///HealthPreferences +<> c__DisplayClass17_0.< RemoveAll > b__0()(at Assets / Scripts / UI / HealthPreferences.cs:90)
            ///UnityEditor.EditorApplication.Internal_CallDelayFunctions()(at < 1135c66e5f4c41a7831fa5798849d8b6 >:0)

            foreach (Transform child in emptyHeartsContainer.transform)
                EditorApplication.delayCall += () => DestroyImmediate(child.gameObject);
#endif
        }
    }

    /// <summary>
    /// 이미지 생성
    /// </summary>
    public void Init(int amount) 
    {
        imagesAmount = amount;
        //Debug.Log("init");
        RemoveAll();

        for (int i = 0; i < imagesAmount; i++)
            CreateImage(i);
    }

    /// <summary>
    /// 스프라이트 생성
    /// </summary>
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

        valuePerImage = maxHealth / imagesAmount;

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

    /// <summary>
    /// 현재 체력만큼 스프라이트 이미지를 갱신하여 만든다
    /// </summary>
    private void UpdateHealth()
    {
        
        valuePerImage = maxHealth / imagesAmount;
        //이미지 당 값 =  기본 체력 /하트 갯수   =33.33 100/3
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
        currentHealth = Mathf.Clamp(amount, 0, maxHealth);
        UpdateHealth();
    }

    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

   
    public float GetTotalHealth()
    {
        return maxHealth;
    }

    
    public void SetTotalHealth(float amount) 
    {
        maxHealth = amount;
        UpdateHealth();
    }

  
    //misc
    //public void AddDamage(float amount)
    //{
        
    //        Debug.Log("HPref-add damage");
    //        currentHealth -= amount;

    //        if (currentHealth < 0)
    //            currentHealth = 0;

    //        UpdateHealth();
        
    //}

    ///// <summary>
    ///// Increase current health by amount
    ///// </summary>
    //public void AddHeal(float amount)
    //{
    //    currentHealth += amount;

    //    if (currentHealth > maxHealth)
    //        currentHealth = maxHealth;

    //    UpdateHealth();
    //}

    ///// <summary>
    ///// Add one image
    ///// </summary>
    //public void AddImage()
    //{
    //    imagesAmount += 1;
    //    Init(imagesAmount);
    //}

    ///// <summary>
    ///// Remove one image
    ///// </summary>
    //public void RemoveImage()
    //{
    //    if (imagesAmount > 0)
    //    {
    //        imagesAmount -= 1;
    //        Init(imagesAmount);
    //    }
    //}

   
    //public void SetFillType(Image.FillMethod type)
    //{
    //    fillMethod = type;
    //    Init(imagesAmount);
    //}

   
}
