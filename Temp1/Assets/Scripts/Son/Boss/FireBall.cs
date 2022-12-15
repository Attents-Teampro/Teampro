using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    public GameObject target;
    public int damage=1;
    public float lifeTime = 4f;
    public float speed =2f;
    ParticleSystem ps;
    Vector3 position;
    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        
    }
    private void Start()
    {
        if(target == null)
        {
            target = FindObjectOfType<Player>().gameObject;
        }
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
        if (other.CompareTag("Player"))
        {
            ICharacter player = other.GetComponent<ICharacter>();
            player.Attacked(damage);
            Debug.Log($"{name} 발사체의 공격 적중");

            Boom();
        }
        else if (other.CompareTag("Enemy"))
        {

        }
        else
        {
            Boom();
        }
        
    }
    void Boom()
    {
        ps.transform.parent = null;
        ps.Play();
        Destroy(ps.gameObject, 4.0f);
        Destroy(gameObject);
    }
}
