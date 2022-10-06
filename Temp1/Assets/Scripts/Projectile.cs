using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour, ICharacter
{
    public int attackDamage = 50;
    private float projectileSpeed = 10f;
    Vector3 targetPoint;

    ICharacter playerCharacter;

    Rigidbody rb;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerCharacter = player.GetComponent<ICharacter>();
        targetPoint = player.position + new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPoint, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //playerCharacter.Attacked(attackDamage);
            Debug.Log("MageAttack");
        }
    }

    public void Die()
    {

    }
    public void Attacked(int damage)
    {

    }

    public void Attack(GameObject target, int damage)
    {

    }
}
