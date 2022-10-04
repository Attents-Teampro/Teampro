using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICharacter
{
    
    /* //추후 캐릭터 HP값들을 참조할 필요성이 생기면 이런 식으로 만들면 좋을 것 같아요~ by 손
    int hp { get; set; }
    int mp { get; set; }
    int damage { get; set; }
    int gold { get; set; }
    */

    void Die();//죽었을 때 실행될 함수
    void Attacked(int damage);//공격받았을 때 실행될 함수
    void Attack(GameObject target, int damage);//공격할 때 실행될 함수
}
