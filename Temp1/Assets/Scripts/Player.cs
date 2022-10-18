using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class Player : MonoBehaviour, ICharacter
{
    //10.11 추가 by 손동욱
    //씬 이동해도 플레이어 유지 및 중복되면 중복 오브젝트를 삭제하기 위한 코드
    public static Player instance;

    public float moveSpeed = 3.0f;
    public float turnSpeed = 10.0f;

    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public int pHP = 100;
    public int damage =1;
    int pDamage = 0;


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

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

    

    private void Awake()
    {
        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //10.11 추가 
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        //by 손동욱 
    }
    void Start()
    {
        equipWeaponIndex = 0;
        equipWeapon = weapons[0].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);
    }

    void Update()
    {
        GetInput();
        //10.11 수정. 기존 Attack 함수가 ICharacter의 Attack함수와 이름 동일하여 기존 Attack함수를 Attacking으로 수정
        Attacking();
        Swap();
        Interation();
        AttackPos();

        if (isFireReady && !isSwap)
        {
            transform.Translate(moveSpeed * Time.deltaTime * inputDir, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);    // 회전 방향 자연스럽게
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Dodge.performed += OnDodge;
        inputActions.Player.Attack.performed += OnAttacking;
    }



    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttacking;
        inputActions.Player.Dodge.performed -= OnDodge;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
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

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
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
    void AttackPos()    // 마우스 방향으로 시야 움직임과 공격
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if (GroupPlane.Raycast(cameraRay, out rayLength))

        {

            Vector3 pointTolook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));

        }
    }

    void OnDodge(InputAction.CallbackContext context)
    {
        if (inputDir != Vector3.zero)
        {
            dodgeVec = inputDir;
            speed *= 2;
            anim.SetTrigger("doDodge");
            Debug.Log("dd");
        }
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

            Invoke("SwapOut", 0.4f);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
        //10.11 임시 추가. 추후 공격 모션에 적용하셔야 몬스터 공격이 실행될 것 같습니다.
        if(collision.gameObject.tag == "Enemy")
        {
            if(equipWeapon == null)
            {
                pDamage = damage;
            }
            else
            {
                pDamage = damage * equipWeapon.iDamage;
            }
            
            Attack(collision.gameObject, pDamage);
            Debug.Log("공격");
        }
        //by 손동욱
    }

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
    public void Die()
    {

    }
    public void Attacked(int d)
    {
        pHP -= d;
    }
    
    public void Attack(GameObject target, int d)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(d);
        //Debug.Log($"{gameObject.name}가 {target.name}을 공격. {d}만큼의 피해를 입혔습니다.\n현재{target.name}의 HP는 {target.GetComponent<Enemy>().curHealth}");
    }
}
