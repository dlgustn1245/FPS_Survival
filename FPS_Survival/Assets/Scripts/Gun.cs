using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;
    public float fireDelay;
    public float reloadDelay;

    public int dmg;

    public int reloadBulletCnt; //총알 재장전 개수
    public int currBullet; //현재 탄알집에 남아있는 총알의 개수
    public int maxBullet; //최대 소유 가능한 총알 개수
    public int carryBullet; //현재 소유하고 있는 총알 개수

    public float retroActionForce; //반동세기
    public float retroActionFineSightForce; //정조준시 반동세기

    public Vector3 fineSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash; //발사 섬광

    public AudioClip fireSound;
}
