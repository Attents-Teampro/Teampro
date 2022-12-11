using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    // 독 늪에 있으면 HP 마이너스

    ParticleSystem poisonCloud;
    Player player;

    private void Start()
    {
        poisonCloud = GetComponent<ParticleSystem>();
    }

    // 파티클에 트리거를 켜서 플레이어가 닿으면 HP 가 닳도록 만듬
    private void OnParticleTrigger()
    {
        if(poisonCloud.CompareTag("Player"))
        {
            player.moveSpeed *= 0.75f;
        }
    }
}
