using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using static UnityEditor.Progress;
using UnityEngine.XR;

//11.04 인터페이스 ICharacter가 빠져있어서 추가했습니다. 
//왜 빼신지 모르겠는데 일단 ICharacter로 enemy랑 enemy_Boss에서 ICharacter로 데미지를 적용하고 있어서
//그게 없으면 에러가 나올 수 밖에 없습니다.(플레이어가 데미지를 입을 때 에러 생성)
//by 손동욱
public class Player : MonoBehaviour, ICharacter, IPlayer
{
    public int countCurrentRoom = 0;
    public AttackState attackState;

    //10.11 추가 by 손동욱
    //씬 이동해도 플레이어 유지 및 중복되면 중복 오브젝트를 삭제하기 위한 코드
    public static Player instance;

    public float moveSpeed = 3.0f;
    public float turnSpeed = 10.0f;
    public float rotateSpeed = 180.0f;

    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public CapsuleCollider dodgeinv;
    public SkinnedMeshRenderer[] skinMesh;
    public MeshRenderer[] meshs;
    

    public AudioClip attackSfx;
    public AudioClip getHitSfx;
    AudioSource audioSource;

    public Image skillFilter;
    //public Text coolTimeCounter; //남은 쿨타임을 표시할 텍스트

    public float coolTime;

    private float currentCoolTime; //남은 쿨타임을 추적 할 변수

    private bool canUseSkill = true; //스킬을 사용할 수 있는지 확인하는 변수


    float moveDir = 0.0f;
    float rotateDir = 0.0f;


    //플레이어 hp와 최대hp 설정 - 양해인 1104
    public int pHP;
    public int pMaxHp = 200;


    public int damage = 1;
    public int pDamage = 0;

    float hAxis;
    float vAxis;

    bool wDown; //걷기
    bool jDown; // 점프
    bool fDown; // 공격
    bool iDown; // 아이템먹기
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady;
    bool isColltime;
    bool isLookAt;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Vector3 inputDir = Vector3.zero;

    Quaternion targetRotation = Quaternion.identity;

    PlayerInputAction inputActions;

    Rigidbody rigid;
    Animator anim;

    EnemyBase enemyBase;

    ParticleSystem weaponPS;
    Transform weapon_r;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

    /// <summary>
    /// 락온 이펙트
    /// </summary>
    LockOnEffect lockOnEffect;

    /// <summary>
    /// 락온 범위
    /// </summary>
    float lockOnRange = 5.0f;

    MainManager mainManager;

    //-------------------------------------------------



    public int attackPower = 10;      // 공격력
    public int defencePower = 3;      // 방어력
    public bool isAlive = true;            // 살아있는지 죽었는지 확인용

    public int AttackPower => attackPower;

    public int DefencePower => defencePower;

    public int PHP
    {
        get => pHP;
        set
        {
            //Debug.Log("공격받음, 이프문 미실행");
            //Debug.Log($"isAlive = {isAlive}\n php = {pHP}\n value = {value}");
            if (isAlive && pHP != value) // 살아있고 HP가 변경되었을 때만 실행
            {
                //Debug.Log("공격받음, 이프문 실행");
                pHP = value;
                anim.SetTrigger("Hit");
                
                StartCoroutine("HitColor");
                if (pHP <= 0)
                {
                    Debug.Log("죽음");
                    Die();
                }
                pHP = Mathf.Clamp(pHP, 0, pMaxHp);

                pHPChange?.Invoke(pHP / pMaxHp);
            }  
        }
    }
    public int MaxHP => pMaxHp;
    public bool IsAlive => isAlive;

    public Action<int> pHPChange { get; set; }
    public Action onDie { get; set; }

    public Transform LockOnTransform => lockOnEffect.transform.parent;

    //--------------------------------------------------------------------

    private void Awake()
    {
        skinMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        mainManager = MainManager.instance;

        lockOnEffect = GetComponentInChildren<LockOnEffect>();
        //LockOff();

        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //SkillBtn p = FindObjectOfType<SkillBtn>();

        dodgeinv = GetComponent<CapsuleCollider>();
        //11.10 추가 by 손동욱
        //mellArea가 null이면 에러가 계속 나와서 수정
        if (dodgeinv == null)
        {
            //Debug.Log("에러, Weapon스크립트의 meleeArea변수가 null입니다.");
        }
        else
        {
            dodgeinv.enabled = true;
        }


            //10.11 추가 
            if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        //by 손동욱 

        //11.11 start에서 이동 by 손동욱
        //플레이어의 hp를 최대 hp로 초기화. 양해인 11.04
        pHP = pMaxHp;
    }
    void Start()
    {
        equipWeaponIndex = 0;
        equipWeapon = weapons[0].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);


        weaponPS = equipWeapon.weaponPS;

        isAlive = true;
        isColltime = false;
        anim.SetBool("IsAlive", isAlive);
        //skillFilter = FindObjectOfType<SkillBtn>().gameObject.GetComponentInChildren<Image>();
        skillFilter = GameObject.Find("SkillFilter").gameObject.GetComponent<Image>();
        if (skillFilter != null)
        {
            skillFilter.fillAmount = 0; //처음에 스킬 버튼을 가리지 않음
        }
        //skillFilter.fillAmount = 0; //처음에 스킬 버튼을 가리지 않음
        //HealthPreferences healthP = FindObjectOfType<HealthPreferences>();
        //healthP.SetPlayer(this);

    }


    private void Update()
    {
        GetInput();
        Swap();
        Interation();
        //UseSkill();
        

    }

    void FixedUpdate()
    {
        
        //10.11 수정. 기존 Attack 함수가 ICharacter의 Attack함수와 이름 동일하여 기존 Attack함수를 Attacking으로 수정
        Attacking();
        //AttackPos();

        if (IsAlive)
        {
            Move();
            Rotate();

            //transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);    // 회전 방향 자연스럽게
        }

        if (isLookAt == true)
        {
            transform.LookAt(nearest);
            transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        HealthPreferences healthP = FindObjectOfType<HealthPreferences>();
        healthP.SetPlayer(this);

        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        //inputActions.Player.Dodge.performed += OnDodge;
        inputActions.Player.DodgeUp.performed += OnDodgeUp;
        inputActions.Player.DodgeDown.performed += OnDodgeDown;
        inputActions.Player.DodgeLeft.performed += OnDodgeLeft;
        inputActions.Player.DodgeRight.performed += OnDodgeRight;
        inputActions.Player.Attack.performed += OnAttacking;
        inputActions.Player.LockOn.performed += OnLockOn;
    }



    private void OnDisable()
    {
        inputActions.Player.LockOn.performed -= OnLockOn;
        inputActions.Player.Attack.performed -= OnAttacking;
        inputActions.Player.DodgeUp.performed -= OnDodgeUp;
        inputActions.Player.DodgeDown.performed -= OnDodgeDown;
        inputActions.Player.DodgeLeft.performed -= OnDodgeLeft;
        inputActions.Player.DodgeRight.performed -= OnDodgeRight;
        //inputActions.Player.Dodge.performed -= OnDodge;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        //if (IsAlive)
        //{
        //    if (isDodge)
        //        inputDir = dodgeVec;

        //    if (isSwap)
        //        moveVec = Vector3.zero;

        //    Vector2 input = context.ReadValue<Vector2>();
        //    inputDir.x = input.x;// * Time.deltaTime;
        //    inputDir.y = 0.0f;
        //    inputDir.z = input.y;// * Time.deltaTime;

        //    if (!context.canceled)
        //    {

        //        Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        //        inputDir = cameraYRotation * inputDir;
        //        targetRotation = Quaternion.LookRotation(inputDir);

        //    }
            

            Vector2 input = context.ReadValue<Vector2>();   // 입력된 값을 읽어오기
            moveDir = input.y;      // w : +1,  s : -1   전진인지 후진인지 결정
            rotateDir = input.x;
            anim.SetBool("isRun", !context.canceled);

            if (isLookAt == true)
            {
                if (IsAlive)
                {
                    if (isDodge)
                        inputDir = dodgeVec;

                    if (isSwap)
                        moveVec = Vector3.zero;

                    Vector2 input2 = context.ReadValue<Vector2>();
                    inputDir.x = input2.x;// * Time.deltaTime;
                    inputDir.y = 0.0f;
                    inputDir.z = input2.y;// * Time.deltaTime;

                    if (!context.canceled)
                    {

                        Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                        inputDir = cameraYRotation * inputDir;
                        targetRotation = Quaternion.LookRotation(inputDir);

                    }
                    anim.SetBool("isRun", !context.canceled);
                }
        }
    }
    void Move()
    {
        // 리지드바디로 이동 설정
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * moveDir * transform.forward);
    }

    void Rotate()
    {
        Quaternion rotate = Quaternion.AngleAxis(rotateDir * rotateSpeed * Time.fixedDeltaTime, transform.up);
        rigid.MoveRotation(rigid.rotation * rotate);
    }




        void GetInput()
    {
        //hAxis = Input.GetAxisRaw("Horizontal");
        //vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    private void OnAttacking(InputAction.CallbackContext obj)
    {
        if (isFireReady && !isDodge && !isSwap && isAlive)
        {
            equipWeapon.Use();
            //int comboState = anim.GetInteger("ComboState"); // ComboState를 애니메이터에서 읽어와서 
            //comboState++;   // 콤보 상태 1 증가 시키기
            //anim.SetInteger("ComboState", comboState);      // 애니메이터에 증가된 콤보 상태 설정

            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            //audioSource.PlayOneShot(attackSfx);
            fireDelay = 0;
        }
    }

    void Attacking()
    {

        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

    }
    //void AttackPos()    // 마우스 방향으로 시야 움직임과 공격
    //{
    //    Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

    //    //float rayLength;

    //    RaycastHit hit;

    //    //if (GroupPlane.Raycast(cameraRay, out rayLength))
    //    if (Physics.Raycast(cameraRay, out hit, 1000, LayerMask.GetMask("floor")))
    //    {
    //        Vector3 pointTolook = hit.point;
    //        Vector3 dir = pointTolook - transform.position;
    //        dir.y = 0;

    //        transform.rotation = Quaternion.LookRotation(dir);
    //    }
    //}

    //void OnDodge(InputAction.CallbackContext context)
    //{
        
    //        UseSkill();
    //    //if (inputDir != Vector3.zero && !isColltime)
    //    //{
    //    //    dodgeVec = inputDir;
    //    //    speed *= 2;
    //    //    anim.SetTrigger("doDodge");
    //    //    StartCoroutine("Dodgeinv");
    //    //    Debug.Log("구르기");
    //    //}
    //}

    void OnDodgeUp(InputAction.CallbackContext context)
    {
        UseSkill();
    }

    void OnDodgeDown(InputAction.CallbackContext context)
    {
        UseSkillDown();
    }

    void OnDodgeLeft(InputAction.CallbackContext context)
    {
        UseSkillLeft();
    }

    void OnDodgeRight(InputAction.CallbackContext context)
    {
        UseSkillRight();
    }

    IEnumerator Dodgeinv()
    {
        yield return new WaitForSeconds(0.0f);  // 0.1초 대기
        dodgeinv.enabled = false;

        yield return new WaitForSeconds(0.9f);
        dodgeinv.enabled = true;
    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //스킬 버튼을 가림
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;
            //coolTimeCounter.text = "" + currentCoolTime;

            //StartCoroutine("CoolTimeCounter");

            canUseSkill = false; //스킬을 사용하면 사용할 수 없는 상태로 바꿈

            //if (inputDir != Vector3.zero && !isColltime)
            //{
            //    dodgeVec = inputDir;
            //    speed *= 2;
            //    anim.SetTrigger("doDodge");
            //    StartCoroutine("Dodgeinv");
            //    Debug.Log("구르기");
            //}

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeUp");
                //StartCoroutine("Dodgeinv");
                //Debug.Log("앞으로구르기");
            }
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }

    public void UseSkillDown()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //스킬 버튼을 가림
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //스킬을 사용하면 사용할 수 없는 상태로 바꿈

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeDown");
                //Debug.Log("뒤로 구르기");
            }
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }

    public void UseSkillLeft()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //스킬 버튼을 가림
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //스킬을 사용하면 사용할 수 없는 상태로 바꿈

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeLeft");
                //Debug.Log("왼쪽으로 구르기");
            }
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }

    public void UseSkillRight()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //스킬 버튼을 가림
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //스킬을 사용하면 사용할 수 없는 상태로 바꿈

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeRight");
                //Debug.Log("오른쪽으로 구르기");
            }
        }
        else
        {
            Debug.Log("아직 스킬을 사용할 수 없습니다.");
        }
    }

    IEnumerator Cooltime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true; //스킬 쿨타임이 끝나면 스킬을 사용할 수 있는 상태로 바꿈

        yield break;
    }

    ////남은 쿨타임을 계산할 코르틴을 만들어줍니다.
    //IEnumerator CoolTimeCounter()
    //{
    //    while (currentCoolTime > 0)
    //    {
    //        yield return new WaitForSeconds(1.0f);

    //        currentCoolTime -= 1.0f;
    //        coolTimeCounter.text = "" + currentCoolTime;
    //    }

    //    yield break;
    //}


    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.1f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    public void Defence(int damage)
    {
        if (isAlive)                // 살아있을 때만 데미지 입음.
        {
            anim.SetTrigger("Hit"); // 피격 애니메이션 재생
            PHP -= (damage);  // 기본공식 = 실제입는 데미지 = 적 공격 데미지 - 방어력
        }
    }
    public void Die()
    {
        isAlive = false;
        anim.SetLayerWeight(1, 0.0f);       // 애니메이션 레이어 가중치 제거
        anim.SetBool("IsAlive", isAlive);   // 죽었다고 표시해서 사망 애니메이션 재생
        onDie?.Invoke();
        mainManager.StageFailed();
    }
    
    IEnumerator HitColor()
    {
        yield return new WaitForSeconds(0.1f);  // 0.1초 대기
        HitColor(true);

        yield return new WaitForSeconds(0.2f);
        HitColor(false);
    }


    void HitColor(bool isHit)
    {
        if (isHit)
        {
            foreach (SkinnedMeshRenderer mesh in skinMesh)
                mesh.material.color = Color.red;
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.red;

        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in skinMesh)
                mesh.material.color = Color.white;
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.tag == "Floor")
    //    //{
    //    //    anim.SetBool("isJump", false);
    //    //    isJump = false;
    //    //}
    //    //10.11 임시 추가. 추후 공격 모션에 적용하셔야 몬스터 공격이 실행될 것 같습니다.
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        if (equipWeapon == null)
    //        {
    //            pDamage = damage;
    //        }
    //        else
    //        {
    //            pDamage = damage * equipWeapon.iDamage;
    //        }

    //        Attack(collision.gameObject, pDamage);
    //        Debug.Log("공격");
    //    }
    //    //by 손동욱
    //}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;


        //Debug.Log(nearObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }

    //10.11 추가. ICharacter 적용
    //프로토타입 제작을 위해서 임시로 플레이어 콜리더에 적 에너미가 들어오면 데미지를 받는 기능 추가
    //추후 공격 콜리더에 적용하거나 해야 될 것 같습니다.
    public void Attacked(int d)
    {
        if(d < 0)
        {
            Heal(d);
        }
        else
        {
            //Debug.Log("어택드 실행");
            //Debug.Log($"현재 플레이어 pHP = {pHP}");
            PHP -= d;
            audioSource.PlayOneShot(getHitSfx);
            //UI에 플레이어 pHP 값을 전달 -양해인 11.04
            Health.instance.SetCurrentHealth(pHP);
        }
    }

    public void Heal(int d)
    {
        PHP -= d;
        Health.instance.SetCurrentHealth(pHP);
    }

    public void Attack(GameObject target, int d)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(d);
        //Debug.Log($"{gameObject.name}가 {target.name}을 공격. {d}만큼의 피해를 입혔습니다.\n현재{target.name}의 HP는 {target.GetComponent<Enemy>().curHealth}");
    }

    public void WeaponEffectSwitch(bool on)
    {
        if (weaponPS != null)
        {
            if (on)
            {
                weaponPS.Play();    // 파티클 이팩트 재생 시작
            }
            else
            {
                weaponPS.Stop();    // 파티클 이팩트 재생 중지
            }
        }
    }

    ///// <summary>
    ///// 무기가 공격 행동을 할 때 무기의 트리거 켜는 함수
    ///// </summary>
    //public void WeaponBladeEnable()
    //{
    //    if (weaponBlade != null)
    //    {
    //        weaponBlade.enabled = true;
    //    }
    //}

    ///// <summary>
    ///// 무기가 공격 행동이 끝날 때 무기의 트리거를 끄는 함수
    ///// </summary>
    //public void WeaponBladeDisable()
    //{
    //    if (weaponBlade != null)
    //    {
    //        weaponBlade.enabled = false;
    //    }
    //}

    public void LockOnToggle()
    {
        LockOn();
    }

    public Transform nearest = null;
    void LockOn()
    {
        isLookAt = true;
        if( nearest != null)
        {
            isLookAt = true;
        }
        
        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("Enemy"));
        if (enemies.Length > 0)
        {           
            float nearestDistance = float.MaxValue;
            foreach (var enemy in enemies)
            {
                Vector3 dir = enemy.transform.position - transform.position;
                float distanceSqr = dir.sqrMagnitude;
                if (dir.sqrMagnitude < nearestDistance)
                {
                    nearestDistance = dir.sqrMagnitude;
                    nearest = enemy.transform;
                    transform.LookAt(nearest.position);
                }
            }

            //Debug.Log($"락온 활성화 {nearest}");
            //lockOnEffect.SetLockOnTarget(nearest);      // 부모지정과 위치 변경
        }
        else
        {
            LockOff();
            Debug.Log("락온 해제");
        }
    }

    void LockOff()
    {
        enemyBase.isDead = true;
        //lockOnEffect.SetLockOnTarget(null);
    }

    private void OnLockOn(InputAction.CallbackContext context)
    {
        LockOnToggle();
        //LockOn();
    }
}
