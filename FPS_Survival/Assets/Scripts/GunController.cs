using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static bool isActivated = false;

    public Gun currGun;
    public Camera cam;
    public GameObject hitEffect;
    public bool isFineSightMode = false; //정조준 모드

    float currFireDelay;

    bool isReload = false;
    
    AudioSource audioSource;
    PlayerController player;
    CrossHair crossHair;
    Vector3 originPos; //본래 포지션 값
    RaycastHit hitInfo;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        crossHair = FindObjectOfType<CrossHair>();
    }

    void Start()
    {
        originPos = Vector3.zero;
    }

    void Update()
    {
        if (isActivated)
        {
            GunFireRate();
            TryFire();
            TryReload();
            TryFineSight();
        }
            
        if (player.isRun) CancelFineSight();
    }

    void GunFireRate()
    {
        if(currFireDelay > 0) currFireDelay -= Time.deltaTime;
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currFireDelay <= 0 && !isReload)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (!isReload && !player.isRun)
        {
            if (currGun.currBullet > 0) Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        crossHair.FireAnim();
        --currGun.currBullet;
        currFireDelay = currGun.fireDelay;
        PlaySoundEffect(currGun.fireSound);
        currGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(RetroAction());
    }

    void Hit()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward + 
            new Vector3(Random.Range(-crossHair.GetAccuracy() - currGun.accuracy, crossHair.GetAccuracy() + currGun.accuracy),
                        Random.Range(-crossHair.GetAccuracy() - currGun.accuracy, crossHair.GetAccuracy() + currGun.accuracy), 
                        0), 
            out hitInfo, currGun.range))
        {
            var hitEffectClone = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            Destroy(hitEffectClone, 0.5f);
        } 
    }

    void TryReload()
    {
        if((Input.GetKeyDown(KeyCode.R) || currGun.currBullet == 0) && !isReload && (currGun.currBullet < currGun.reloadBulletCnt))
        {
            CancelFineSight();
            StartCoroutine(Reload());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    IEnumerator Reload()
    {
        if (currGun.carryBullet > 0)
        {
            isReload = true;
            currGun.anim.SetTrigger("Reload");

            currGun.carryBullet += currGun.currBullet;
            currGun.currBullet = 0;

            yield return new WaitForSeconds(currGun.reloadDelay);

            if(currGun.carryBullet >= currGun.reloadBulletCnt)
            {
                currGun.currBullet = currGun.reloadBulletCnt;
                currGun.carryBullet -= currGun.reloadBulletCnt;
            }
            else
            {
                currGun.currBullet = currGun.carryBullet;
                currGun.carryBullet = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("no Bullets");
        }
    }

    void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        } 
    }

    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currGun.anim.SetBool("FineSightMode", isFineSightMode);
        crossHair.FineSightAnim(isFineSightMode);
        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivate());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivate());
        }
    }

    IEnumerator FineSightActivate()
    {
        while(currGun.transform.localPosition != currGun.fineSightOriginPos)
        {
            currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, currGun.fineSightOriginPos, 0.05f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivate()
    {
        while (currGun.transform.localPosition != originPos)
        {
            currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, originPos, 0.05f);
            yield return null;
        }
    }

    IEnumerator RetroAction()
    {
        Vector3 recoilBack = new Vector3(currGun.retroActionForce, originPos.y, originPos.z); //원래는 z축 반동인데 90도 회전해서 x축반동 
        Vector3 retroActionRecoilBack = new Vector3(currGun.retroActionFineSightForce, currGun.fineSightOriginPos.y, currGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currGun.transform.localPosition = originPos;

            while (currGun.transform.localPosition.x <= currGun.retroActionForce - 0.02f)
            {
                currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            while(currGun.transform.localPosition != originPos)
            {
                currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currGun.transform.localPosition = currGun.fineSightOriginPos;

            while (currGun.transform.localPosition.x <= currGun.retroActionFineSightForce - 0.02f)
            {
                currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            while (currGun.transform.localPosition != originPos)
            {
                currGun.transform.localPosition = Vector3.Lerp(currGun.transform.localPosition, currGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    void PlaySoundEffect(AudioClip myClip)
    {
        audioSource.clip = myClip;
        audioSource.Play();
    }

    public Gun GetGun()
    {
        return currGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun gun)
    {
        if (WeaponManager.currWeapon) WeaponManager.currWeapon.gameObject.SetActive(false);

        currGun = gun;
        WeaponManager.currWeapon = currGun.GetComponent<Transform>();
        WeaponManager.currWeaponAnim = currGun.anim;

        currGun.transform.localPosition = Vector3.zero;
        currGun.gameObject.SetActive(true);
        isActivated = true;
    }
}
