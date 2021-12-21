using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;

    public float range;
    public int dmg;
    public float workSpeed;
    public float attackDelay;
    public float attackDelayA; //공격 활성화 시점
    public float attackDelayB; //공격 비활성화 시점

    //근접무기 유형
    public bool isAxe;
    public bool isPickAxe;
    public bool isHand;

    public Animator anim;
}
