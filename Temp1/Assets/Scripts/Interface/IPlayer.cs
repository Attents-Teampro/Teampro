using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IPlayer : ICharacter
{
    //int PHP { get; set; }  // HP�� Ȯ���ϰ� ������ �� �ִ�
    //int MaxHP { get; }    // �ִ�HP�� Ȯ���� �� �ִ�.

    ///// <summary>
    ///// HP�� ���汮 �� ����� ��������Ʈ�� ������Ƽ
    ///// �Ķ���ʹ� ����/�ִ� ����.
    ///// </summary>
    //Action<int> onHealthChange { get; set; }

    Action onDie { get; set; }   // �׾��� �� ����� �����Ը�Ʈ�� ������Ƽ
}
