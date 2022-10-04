using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ICharacter
{
    public float attackDamage = 50f;
    private float projectileSpeed = 10f;

    Rigidbody rb;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position+new Vector3(0,1f,0), projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void ICharacter.Die()
    {
        
    }
    void ICharacter.Attacked(int damage)
    {
       
    }

    void ICharacter.Attack(GameObject target, int damage)
    {
        
    }
}
