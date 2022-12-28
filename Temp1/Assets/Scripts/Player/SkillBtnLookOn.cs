using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtnLookOn : MonoBehaviour
{
    public Image skillFilterLookOn;
    public Text coolTimeCounter; //���� ��Ÿ���� ǥ���� �ؽ�Ʈ

    public float coolTime;

    private float currentCoolTime; //���� ��Ÿ���� ���� �� ����

    private bool canUseSkill = true; //��ų�� ����� �� �ִ��� Ȯ���ϴ� ����

    void start()
    {
        skillFilterLookOn.fillAmount = 0; //ó���� ��ų ��ư�� ������ ����
    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Use Skill");
            skillFilterLookOn.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");

            currentCoolTime = coolTime;
            coolTimeCounter.text = "" + currentCoolTime;

            StartCoroutine("CoolTimeCounter");

            canUseSkill = false; //��ų�� ����ϸ� ����� �� ���� ���·� �ٲ�
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    IEnumerator Cooltime()
    {
        while (skillFilterLookOn.fillAmount > 0)
        {
            skillFilterLookOn.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + currentCoolTime;
        }

        yield break;
    }
}
