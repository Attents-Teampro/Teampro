using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    // �� �˿� ������ HP ���̳ʽ�

    ParticleSystem poisonCloud;
    Player player;

    private void Start()
    {
        poisonCloud = GetComponent<ParticleSystem>();
    }

    // ��ƼŬ�� Ʈ���Ÿ� �Ѽ� �÷��̾ ������ HP �� �⵵�� ����
    private void OnParticleTrigger()
    {
        if(poisonCloud.CompareTag("Player"))
        {
            player.moveSpeed *= 0.75f;
        }
    }
}
