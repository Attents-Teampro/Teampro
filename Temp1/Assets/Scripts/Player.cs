using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

//11.04 인터페이스 ICharacter가 빠져있어서 추가했습니다. 
//왜 빼신지 모르겠는데 일단 ICharacter로 enemy랑 enemy_Boss에서 ICharacter로 데미지를 적용하고 있어서
//그게 없으면 에러가 나올 수 밖에 없습니다.(플레이어가 데미지를 입을 때 에러 생성)
//by 손동욱
public class Player : MonoBehaviour, ICharacter
{
    public int countCurrentRoom = 0;
    public AttackState attackState;

    //10.11 추가 by 손동욱
    //씬 이동해도 플레이어 유지 및 중복되면 중복 오브젝트를 삭제하기 위한 코드
    public static Player instance;

    public float moveSpeed = 3.0f;
    public float turnSpeed = 10.0f;

    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public CapsuleCollider dodgeinv;

     

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

    Vector3 moveVec;
    Vector3 dodgeVec;

    Vector3 inputDir = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;

    PlayerInputAction inputActions;

    Rigidbody rigid;
    Animator anim;

    ParticleSystem weaponPS;
    Transform weapon_r;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

    ///// <summary>
    ///// 락온 이펙트
    ///// </summary>
    //LockOnEffect lockOnEffect;

    ///// <summary>
    ///// 락온 범위
    ///// </summary>
    //float lockOnRange = 5.0f;

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
            Debug.Log("공격받음, 이프문 미실행");
            Debug.Log($"isAlive = {isAlive}\n php = {pHP}\n value = {value}");
            if (isAlive && pHP != value) // 살아있고 HP가 변경되었을 때만 실행
            {
                Debug.Log("공격받음, 이프문 실행");
                pHP = value;
                anim.SetTrigger("Hit");
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

    //public Transform LockOnTransform => lockOnEffect.transform.parent;

    //--------------------------------------------------------------------

    private void Awake()
    {

        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

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
        anim.SetBool("IsAlive", isAlive);

        //HealthPreferences healthP = FindObjectOfType<HealthPreferences>();
        //healthP.SetPlayer(this);

    }


    private void Update()
    {
        GetInput();
        Swap();
        Interation();

    }

    void FixedUpdate()
    {

        //10.11 수정. 기존 Attack 함수가 ICharacter의 Attack함수와 이름 동일하여 기존 Attack함수를 Attacking으로 수정
        Attacking();
        //AttackPos();

        if (IsAlive)
        {
            transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);    // 회전 방향 자연스럽게
        }
    }

    private void OnEnable()
    {
        HealthPreferences healthP = FindObjectOfType<HealthPreferences>();
        healthP.SetPlayer(this);

        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dodge.performed += OnDodge;
        inputActions.Player.Attack.performed += OnAttacking;
        //inputActions.Player.LockOn.performed += OnLockOn;
    }



    private void OnDisable()
    {
        //inputActions.Player.LockOn.performed -= OnLockOn;
        inputActions.Player.Attack.performed -= OnAttacking;
        inputActions.Player.Dodge.performed -= OnDodge;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (IsAlive)
        {
            if (isDodge)
                inputDir = dodgeVec;

            if (isSwap)
                moveVec = Vector3.zero;

            Vector2 input = context.ReadValue<Vector2>();
            inputDir.x = input.x;
            inputDir.y = 0.0f;
            inputDir.z = input.y;

            if (!context.canceled)
            {
                Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                inputDir = cameraYRotation * inputDir;

                targetRotation = Quaternion.LookRotation(inputDir);
            }
            anim.SetBool("isRun", !context.canceled);
        }
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
        if (isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
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

    void OnDodge(InputAction.CallbackContext context)
    {
        if (inputDir != Vector3.zero)
        {
            dodgeVec = inputDir;
            speed *= 2;
            anim.SetTrigger("doDodge");
            StartCoroutine("Dodgeinv");
            Debug.Log("구르기");
            
        }
    }

    IEnumerator Dodgeinv()
    {
        yield return new WaitForSeconds(0.0f);  // 0.1초 대기
        dodgeinv.enabled = false;

        yield return new WaitForSeconds(0.9f);
        dodgeinv.enabled = true;
    }


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
        Debug.Log("어택드 실행");
        Debug.Log($"현재 플레이어 pHP = {pHP}");
        PHP -= d;
        //UI에 플레이어 pHP 값을 전달 -양해인 11.04
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

    //public void LockOnToggle()
    //{
    //    LockOn();

    //    //if(lockOnEffect.activeSelf)
    //    //{
    //    //    LockOff();
    //    //}
    //    //else
    //    //{
    //    //    LockOn();
    //    //}
    //}

    //void LockOn()
    //{
    //    // lockOnRange 거리 안에 있는 Enemy오브젝트 찾기
    //    Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("Enemy"));
    //    if (enemies.Length > 0)
    //    {
    //        // 찾은 적 중 가장 가까이 있는 적 찾기
    //        Transform nearest = null;
    //        float nearestDistance = float.MaxValue;
    //        foreach (var enemy in enemies)
    //        {
    //            Vector3 dir = enemy.transform.position - transform.position;
    //            float distanceSqr = dir.sqrMagnitude;   // root 연산은 느리기 때문에 sqrMagnitude 사용
    //            if (dir.sqrMagnitude < nearestDistance)
    //            {
    //                nearestDistance = dir.sqrMagnitude;
    //                nearest = enemy.transform;
    //            }
    //        }

    //        // 가장 가까이에 있는 적에세 LockOnEffect 붙이기
    //        lockOnEffect.SetLockOnTarget(nearest);      // 부모지정과 위치 변경
    //    }
    //    else
    //    {
    //        LockOff();  // 주면에 적이 없는데 락온을 시도하면 락온 해제
    //    }
    //}

    //void LockOff()
    //{
    //    lockOnEffect.SetLockOnTarget(null);
    //}

    //private void OnLockOn(InputAction.CallbackContext context)
    //{
    //    LockOnToggle();
    //}
}
