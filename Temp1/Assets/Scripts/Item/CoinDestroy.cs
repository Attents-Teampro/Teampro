using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroy : MonoBehaviour
{
    public int coinValue;
    //public AudioClip sfx_Coin;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = transform.GetChild(3).GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            audioSource.transform.parent= null;
            MainManager.instance.gold += coinValue;
            Debug.Log($"현재 플레이어 골드 : {MainManager.instance.gold}");
            Destroy(this.gameObject);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}
