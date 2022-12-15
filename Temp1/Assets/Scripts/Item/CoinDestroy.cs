using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroy : MonoBehaviour
{
    public int coinValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MainManager.instance.gold += coinValue;
            Destroy(this.gameObject);
            Debug.Log($"현재 플레이어 골드 : {MainManager.instance.gold}");
        }
    }
}
