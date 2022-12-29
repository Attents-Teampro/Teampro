using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public GameObject target;
    [SerializeField]
    public int damage=1;
    [SerializeField]
    float lifeTime = 4f;
    [SerializeField]
    float speed =2f;
    public Enemy_Boss boss;
    ParticleSystem ps;
    Vector3 position;
    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        if (ps == null)
        {
            ps = transform.GetChild(transform.childCount -1).GetComponent<ParticleSystem>();
        }
        
    }
    private void Start()
    {
        if(target == null)
        {
            target = FindObjectOfType<Player>().gameObject;
        }
        StartCoroutine(LifeTimeBoom());
        Destroy(gameObject, lifeTime);
        position = target.transform.position;
        position.y = transform.position.y;
        transform.LookAt(position);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Time.deltaTime * speed * transform.forward, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("wjqchr");
        if (other.CompareTag("Player"))
        {
            ICharacter player = other.GetComponent<ICharacter>();
            player.Attacked(damage);
            //Debug.Log($"{name} 발사체의 공격 적중");

            Boom();
        }
        else if (other.CompareTag("Enemy")) //|| other.CompareTag("Wall"))
        {
            //Boom();
        }
        else
        {
            
        }
        
    }
    void Boom(float lifeTime = 10f)
    {
        ps.gameObject.SetActive(true);
        if (ps !=null && ps.transform != null)
        {
            if (ps.transform.parent != null)
            {
                ps.transform.parent = null;
                ps.Play();
            }
        }
        
        
        boss.PlayExplosionAudio();
        if(ps != null)
        {
            Destroy(ps.gameObject, lifeTime);
        }
        
        Destroy(gameObject);
    }
    IEnumerator LifeTimeBoom()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject != null)
        {
            Boom();
        }
        
    }
}
