﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    public static bool isActivated = false;

    /*void Start()
    {
        WeaponManager.currWeapon = currCloseWeapon.GetComponent<Transform>();
        WeaponManager.currWeaponAnim = currCloseWeapon.anim;
    }*/

    void Update()
    {
        if (isActivated) TryAttack();
    }

    protected override IEnumerator Hit()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActivated = true;
    }
}
