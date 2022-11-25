using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IPlayer : ICharacter
{
    //int PHP { get; set; }  // HP를 확인하고 설정할 수 있다
    //int MaxHP { get; }    // 최대HP를 확인할 수 있다.

    ///// <summary>
    ///// HP가 변경괼 때 실행될 델리게이트용 프로퍼티
    ///// 파라메터는 현재/최대 비율.
    ///// </summary>
    //Action<int> onHealthChange { get; set; }

    Action onDie { get; set; }   // 죽었을 때 실행될 델리게리트용 프로퍼티
}
