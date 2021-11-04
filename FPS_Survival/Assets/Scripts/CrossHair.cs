using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public Animator anim;
    public GameObject crossHairHUD;
    public GunController gunController;

    float gunAccuracy;

    void Awake()
    {
        gunController = FindObjectOfType<GunController>();
    }

    public void WalkingAnim(bool flag)
    {
        anim.SetBool("Walking", flag);
    }

    public void RunningAnim(bool flag)
    {
        anim.SetBool("Running", flag);
    }

    public void CrouchingAnim(bool flag)
    {
        anim.SetBool("Crouching", flag);
    }

    public void FineSightAnim(bool flag)
    {
        anim.SetBool("FineSight", flag);
    }

    public void FireAnim()
    {
        if (anim.GetBool("Walking"))
        {
            anim.SetTrigger("Walk_Fire");
        }
        else if (anim.GetBool("Crouching"))
        {
            anim.SetTrigger("Crouch_Fire");
        }
        else
        {
            anim.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking"))
        {
            gunAccuracy = 0.06f;
        }
        else if (anim.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        }
        else if(gunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.035f;
        }

        return gunAccuracy;
    }
}
