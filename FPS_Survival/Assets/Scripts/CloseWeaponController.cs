using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상 클래스 -> 미완성 클래스, 추상 메소드를 선언(구현 x, 추상 메소드 -> 자식 클래스에서 구현)
public abstract class CloseWeaponController : MonoBehaviour
{
    public CloseWeapon currCloseWeapon;

    protected bool isAttack = false; //protected : 자식 클래스에서만 접근 가능
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    protected IEnumerator Attack()
    {
        isAttack = true;
        currCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currCloseWeapon.attackDelayA);
        isSwing = true;

        StartCoroutine(Hit());

        yield return new WaitForSeconds(currCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currCloseWeapon.attackDelay - currCloseWeapon.attackDelayA - currCloseWeapon.attackDelayB);
        isAttack = false;
    }

    //추상 코루틴 -> 미완성(자식 클래스에서 구현)
    protected abstract IEnumerator Hit();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currCloseWeapon.range)) return true;
        return false;
    }

    //가상 함수 : 완성 함수이지만 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currWeapon) WeaponManager.currWeapon.gameObject.SetActive(false);

        currCloseWeapon = closeWeapon;
        WeaponManager.currWeapon = currCloseWeapon.GetComponent<Transform>();
        WeaponManager.currWeaponAnim = currCloseWeapon.anim;

        currCloseWeapon.transform.localPosition = Vector3.zero;
        currCloseWeapon.gameObject.SetActive(true);
    }
}
