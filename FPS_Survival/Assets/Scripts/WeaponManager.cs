using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon; //공유 자원->무기 중복 교체 실행 방지
    public static Transform currWeapon;
    public static Animator currWeaponAnim;

    public string currWeaponType;
    public float changingDelay;
    public float changeEndDelay;

    public GunController gunController;
    public HandController handController;
    public AxeController axeController;
    public PickaxeController pickaxeController;

    public Gun[] guns;
    public CloseWeapon[] hands;
    public CloseWeapon[] axes;
    public CloseWeapon[] pickaxes;

    Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    Dictionary<string, CloseWeapon> handDic = new Dictionary<string, CloseWeapon>();
    Dictionary<string, CloseWeapon> axeDic = new Dictionary<string, CloseWeapon>();
    Dictionary<string, CloseWeapon> pickaxeDic = new Dictionary<string, CloseWeapon>();

    void Start()
    {
        for(int i = 0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);
        }   
        for(int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDic.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDic.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ChangeWeapon("HAND", "맨손"));
            else if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(ChangeWeapon("GUN", "SubMachineGun1"));
            else if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(ChangeWeapon("AXE", "Axe"));
            else if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(ChangeWeapon("PICKAXE", "Pickaxe"));
        }
    }

    public IEnumerator ChangeWeapon(string type, string name)
    {
        isChangeWeapon = true;
        currWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changingDelay);

        CancelPrevWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeEndDelay);

        currWeaponType = type;
        isChangeWeapon = false;
    }

    void CancelPrevWeaponAction()
    {
        switch (currWeaponType)
        {
            case "GUN":
                gunController.CancelFineSight(); //정조준 취소
                gunController.CancelReload(); //재장전 취소
                GunController.isActivated = false;
                break;
            case "HAND":
                HandController.isActivated = false;
                break;
            case "AXE":
                AxeController.isActivated = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivated = false;
                break;
        }
    }

    void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            gunController.GunChange(gunDic[name]);
        }   
        else if(type == "HAND")
        {
            handController.CloseWeaponChange(handDic[name]);
        }
        else if (type == "AXE")
        {
            axeController.CloseWeaponChange(axeDic[name]);
        }
        else if (type == "PICKAXE")
        {
            pickaxeController.CloseWeaponChange(pickaxeDic[name]);
        }
    }
}
