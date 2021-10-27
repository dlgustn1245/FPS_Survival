using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Hand currHand;

    bool isAttack = false;
    bool isSwing = false;

    RaycastHit hitInfo;

    void Update()
    {
        TryAttack();
    }

    void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        currHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currHand.attackDelayA);
        isSwing = true;
        StartCoroutine(Hit());

        yield return new WaitForSeconds(currHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currHand.attackDelay - currHand.attackDelayA - currHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator Hit()
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

    bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currHand.range)) return true;
        return false;
    }
}
