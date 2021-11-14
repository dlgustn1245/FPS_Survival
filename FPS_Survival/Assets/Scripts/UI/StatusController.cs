using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    public int hp; //체력
    public int sp; //스태미나
    public int spIncreaseSpeed; //스태미나 증가량
    public int spRechargeTime; //스태미나 재회복 딜레이
    public int dp; //방어력
    public int hungry; //허기
    public int thirsty; //갈증
    public int hungryDecreaseTime; //허기가 줄어드는 속도
    public int thirstyDecreaseTime; //목마름이 줄어드는 속도
    public int satisfy; //만족도
    int currHp;
    int currSp;
    int currSpRechargeTime;
    int currDp;
    int currHungry;
    int currThirsty;
    int currHungryDecreaseTime;
    int currThirstyDecreaseTime;
    int currSatisfy;
    bool spUsed; //스태미나 감소 여부

    public Image[] images_Gauge;

    const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        currHp = hp;
        currSp = sp;
        currDp = dp;
        currHungry = hungry;
        currThirsty = thirsty;
        currSatisfy = satisfy;
    }


    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currSpRechargeTime < spRechargeTime) ++currSpRechargeTime;
            else spUsed = false;
        }
    }

    void SPRecover()
    {
        if (!spUsed && currSp < sp)
        {
            currSp += spIncreaseSpeed;
        }
    }

    void Hungry()
    {
        if (currHungry > 0)
        {
            if (currHungryDecreaseTime <= hungryDecreaseTime) ++currHungryDecreaseTime;
            else
            {
                --currHungry;
                currHungryDecreaseTime = 0;
            }
        }
        else Debug.Log("배고픔 수치 = 0");
    }

    void Thirsty()
    {
        if (currThirsty > 0)
        {
            if (currThirstyDecreaseTime <= thirstyDecreaseTime) ++currThirstyDecreaseTime;
            else
            {
                --currThirsty;
                currThirstyDecreaseTime = 0;
            }
        }
        else Debug.Log("목마름 수치 = 0");
    }

    void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currHp / hp;
        images_Gauge[DP].fillAmount = (float)currDp / dp;
        images_Gauge[SP].fillAmount = (float)currSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currSatisfy / satisfy;
    }

    public void IncreaseHP(int amount)
    {
        if (currHp + amount < hp) currHp += amount;
        else currHp = hp;
    }

    public void DecreaseHP(int amount)
    {
        if (currDp > 0)
        {
            DecreaseDP(amount);
            return;
        }
        currHp -= amount;
        if (currHp <= 0) Debug.Log("캐릭터의 체력이 0이 됨");
    }

    public void IncreaseDP(int amount)
    {
        if (currDp + amount < dp) currDp += amount;
        else currDp = dp;
    }

    public void DecreaseDP(int amount)
    {
        currDp -= amount;
        if (currDp <= 0) Debug.Log("캐릭터의 방어력이 0이 됨");
    }

    public void IncreaseHungry(int amount)
    {
        if (currHungry + amount < hungry) currHungry += amount;
        else currHungry = hungry;
    }

    public void DecreaseHungry(int amount)
    {
        if(currHungry - amount < 0) currHungry = 0; 
        else currHungry -= amount;
    }

    public void IncreaseThirsty(int amount)
    {
        if (currThirsty + amount < thirsty) currThirsty += amount;
        else currThirsty = thirsty;
    }

    public void DecreaseThirsty(int amount)
    {
        if (currThirsty - amount < 0) currThirsty = 0;
        else currThirsty -= amount;
    }

    public void DecreaseStamina(int amount)
    {
        spUsed = true;
        currSpRechargeTime = 0;

        currSp = (currSp - amount > 0) ? currSp - amount : 0;
        print(currSp);
    }

    public int GetCurrentSP()
    {
        return currSp;
    }
}
