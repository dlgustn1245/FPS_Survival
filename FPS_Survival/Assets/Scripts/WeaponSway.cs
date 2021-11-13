using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public Vector3 limitPos;
    public Vector3 fineSightLimitPos;
    public Vector3 smoothSway; // 부드러운 움직임 정도
    public GunController gunControler;

    Vector3 originPos;
    Vector3 currPos;

    void Start()
    {
        originPos = this.transform.localPosition;
    }

    void Update()
    {
        TrySway();
    }

    void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) Swaying();
        else BackToOriginPos();
    }

    void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        if (!gunControler.isFineSightMode)
        {
            currPos.Set(Mathf.Clamp(Mathf.Lerp(currPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                    Mathf.Clamp(Mathf.Lerp(currPos.y, -moveY, smoothSway.x), -limitPos.y, limitPos.y),
                    originPos.z); //z값은 처음 설정된 값 그대로 사용
        }
        else
        {
            currPos.Set(Mathf.Clamp(Mathf.Lerp(currPos.x, -moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                    Mathf.Clamp(Mathf.Lerp(currPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                    originPos.z); //z값은 처음 설정된 값 그대로 사용
        }

        this.transform.localPosition = currPos;
    }

    void BackToOriginPos()
    {
        currPos = Vector3.Lerp(currPos, originPos, smoothSway.x);
        this.transform.localPosition = currPos;
    }
}
