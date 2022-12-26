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

//11.04 �������̽� ICharacter�� �����־ �߰��߽��ϴ�. 
//�� ������ �𸣰ڴµ� �ϴ� ICharacter�� enemy�� enemy_Boss���� ICharacter�� �������� �����ϰ� �־
//�װ� ������ ������ ���� �� �ۿ� �����ϴ�.(�÷��̾ �������� ���� �� ���� ����)
//by �յ���
public class Player : MonoBehaviour, ICharacter, IPlayer
{
    public int countCurrentRoom = 0;
    public AttackState attackState;

    //10.11 �߰� by �յ���
    //�� �̵��ص� �÷��̾� ���� �� �ߺ��Ǹ� �ߺ� ������Ʈ�� �����ϱ� ���� �ڵ�
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
    //public Text coolTimeCounter; //���� ��Ÿ���� ǥ���� �ؽ�Ʈ

    public float coolTime;

    private float currentCoolTime; //���� ��Ÿ���� ���� �� ����

    private bool canUseSkill = true; //��ų�� ����� �� �ִ��� Ȯ���ϴ� ����


    float moveDir = 0.0f;
    float rotateDir = 0.0f;


    //�÷��̾� hp�� �ִ�hp ���� - ������ 1104
    public int pHP;
    public int pMaxHp = 200;


    public int damage = 1;
    public int pDamage = 0;

    float hAxis;
    float vAxis;

    bool wDown; //�ȱ�
    bool jDown; // ����
    bool fDown; // ����
    bool iDown; // �����۸Ա�
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
    /// ���� ����Ʈ
    /// </summary>
    LockOnEffect lockOnEffect;

    /// <summary>
    /// ���� ����
    /// </summary>
    float lockOnRange = 5.0f;

    MainManager mainManager;

    //-------------------------------------------------



    public int attackPower = 10;      // ���ݷ�
    public int defencePower = 3;      // ����
    public bool isAlive = true;            // ����ִ��� �׾����� Ȯ�ο�

    public int AttackPower => attackPower;

    public int DefencePower => defencePower;

    public int PHP
    {
        get => pHP;
        set
        {
            //Debug.Log("���ݹ���, ������ �̽���");
            //Debug.Log($"isAlive = {isAlive}\n php = {pHP}\n value = {value}");
            if (isAlive && pHP != value) // ����ְ� HP�� ����Ǿ��� ���� ����
            {
                //Debug.Log("���ݹ���, ������ ����");
                pHP = value;
                anim.SetTrigger("Hit");
                
                StartCoroutine("HitColor");
                if (pHP <= 0)
                {
                    Debug.Log("����");
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
        //11.10 �߰� by �յ���
        //mellArea�� null�̸� ������ ��� ���ͼ� ����
        if (dodgeinv == null)
        {
            //Debug.Log("����, Weapon��ũ��Ʈ�� meleeArea������ null�Դϴ�.");
        }
        else
        {
            dodgeinv.enabled = true;
        }


            //10.11 �߰� 
            if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        //by �յ��� 

        //11.11 start���� �̵� by �յ���
        //�÷��̾��� hp�� �ִ� hp�� �ʱ�ȭ. ������ 11.04
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
            skillFilter.fillAmount = 0; //ó���� ��ų ��ư�� ������ ����
        }
        //skillFilter.fillAmount = 0; //ó���� ��ų ��ư�� ������ ����
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
        
        //10.11 ����. ���� Attack �Լ��� ICharacter�� Attack�Լ��� �̸� �����Ͽ� ���� Attack�Լ��� Attacking���� ����
        Attacking();
        //AttackPos();

        if (IsAlive)
        {
            Move();
            Rotate();

            //transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);    // ȸ�� ���� �ڿ�������
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
            

            Vector2 input = context.ReadValue<Vector2>();   // �Էµ� ���� �о����
            moveDir = input.y;      // w : +1,  s : -1   �������� �������� ����
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
        // ������ٵ�� �̵� ����
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
            //int comboState = anim.GetInteger("ComboState"); // ComboState�� �ִϸ����Ϳ��� �о�ͼ� 
            //comboState++;   // �޺� ���� 1 ���� ��Ű��
            //anim.SetInteger("ComboState", comboState);      // �ִϸ����Ϳ� ������ �޺� ���� ����

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
    //void AttackPos()    // ���콺 �������� �þ� �����Ӱ� ����
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
    //    //    Debug.Log("������");
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
        yield return new WaitForSeconds(0.0f);  // 0.1�� ���
        dodgeinv.enabled = false;

        yield return new WaitForSeconds(0.9f);
        dodgeinv.enabled = true;
    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;
            //coolTimeCounter.text = "" + currentCoolTime;

            //StartCoroutine("CoolTimeCounter");

            canUseSkill = false; //��ų�� ����ϸ� ����� �� ���� ���·� �ٲ�

            //if (inputDir != Vector3.zero && !isColltime)
            //{
            //    dodgeVec = inputDir;
            //    speed *= 2;
            //    anim.SetTrigger("doDodge");
            //    StartCoroutine("Dodgeinv");
            //    Debug.Log("������");
            //}

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeUp");
                //StartCoroutine("Dodgeinv");
                //Debug.Log("�����α�����");
            }
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void UseSkillDown()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //��ų�� ����ϸ� ����� �� ���� ���·� �ٲ�

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeDown");
                //Debug.Log("�ڷ� ������");
            }
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void UseSkillLeft()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //��ų�� ����ϸ� ����� �� ���� ���·� �ٲ�

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeLeft");
                //Debug.Log("�������� ������");
            }
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void UseSkillRight()
    {
        if (canUseSkill)
        {
            //Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;

            canUseSkill = false; //��ų�� ����ϸ� ����� �� ���� ���·� �ٲ�

            inputDir = new Vector3(moveDir, 0, rotateDir);
            if (inputDir != Vector3.zero && !isColltime)
            {
                dodgeVec = inputDir;
                speed *= 2;
                anim.SetTrigger("doDodgeRight");
                //Debug.Log("���������� ������");
            }
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    IEnumerator Cooltime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    ////���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
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
        if (isAlive)                // ������� ���� ������ ����.
        {
            anim.SetTrigger("Hit"); // �ǰ� �ִϸ��̼� ���
            PHP -= (damage);  // �⺻���� = �����Դ� ������ = �� ���� ������ - ����
        }
    }
    public void Die()
    {
        isAlive = false;
        anim.SetLayerWeight(1, 0.0f);       // �ִϸ��̼� ���̾� ����ġ ����
        anim.SetBool("IsAlive", isAlive);   // �׾��ٰ� ǥ���ؼ� ��� �ִϸ��̼� ���
        onDie?.Invoke();
        mainManager.StageFailed();
    }
    
    IEnumerator HitColor()
    {
        yield return new WaitForSeconds(0.1f);  // 0.1�� ���
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
    //    //10.11 �ӽ� �߰�. ���� ���� ��ǿ� �����ϼž� ���� ������ ����� �� �����ϴ�.
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
    //        Debug.Log("����");
    //    }
    //    //by �յ���
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

    //10.11 �߰�. ICharacter ����
    //������Ÿ�� ������ ���ؼ� �ӽ÷� �÷��̾� �ݸ����� �� ���ʹ̰� ������ �������� �޴� ��� �߰�
    //���� ���� �ݸ����� �����ϰų� �ؾ� �� �� �����ϴ�.
    public void Attacked(int d)
    {
        if(d < 0)
        {
            Heal(d);
        }
        else
        {
            //Debug.Log("���õ� ����");
            //Debug.Log($"���� �÷��̾� pHP = {pHP}");
            PHP -= d;
            audioSource.PlayOneShot(getHitSfx);
            //UI�� �÷��̾� pHP ���� ���� -������ 11.04
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
        //Debug.Log($"{gameObject.name}�� {target.name}�� ����. {d}��ŭ�� ���ظ� �������ϴ�.\n����{target.name}�� HP�� {target.GetComponent<Enemy>().curHealth}");
    }

    public void WeaponEffectSwitch(bool on)
    {
        if (weaponPS != null)
        {
            if (on)
            {
                weaponPS.Play();    // ��ƼŬ ����Ʈ ��� ����
            }
            else
            {
                weaponPS.Stop();    // ��ƼŬ ����Ʈ ��� ����
            }
        }
    }

    ///// <summary>
    ///// ���Ⱑ ���� �ൿ�� �� �� ������ Ʈ���� �Ѵ� �Լ�
    ///// </summary>
    //public void WeaponBladeEnable()
    //{
    //    if (weaponBlade != null)
    //    {
    //        weaponBlade.enabled = true;
    //    }
    //}

    ///// <summary>
    ///// ���Ⱑ ���� �ൿ�� ���� �� ������ Ʈ���Ÿ� ���� �Լ�
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

            //Debug.Log($"���� Ȱ��ȭ {nearest}");
            //lockOnEffect.SetLockOnTarget(nearest);      // �θ������� ��ġ ����
        }
        else
        {
            LockOff();
            Debug.Log("���� ����");
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
