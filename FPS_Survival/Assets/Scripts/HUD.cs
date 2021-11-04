using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text[] text_Bullet;

    public GunController gunController;
    public GameObject bulletHUD;

    Gun currGun;

    void Update()
    {
        CheckBullets();
    }

    void CheckBullets()
    {
        currGun = gunController.GetGun();
        text_Bullet[0].text = currGun.carryBullet.ToString();
        text_Bullet[1].text = currGun.reloadBulletCnt.ToString();
        text_Bullet[2].text = currGun.currBullet.ToString();
    }
}
