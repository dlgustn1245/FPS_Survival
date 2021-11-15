using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public SphereCollider col;
    public GameObject rock; //바위
    public GameObject debris; // 깨진 바위
    public GameObject rock_effect; //채굴 이펙트
    public GameObject rock_Item_Prefab;
    public int hp;
    public int count;
    public float destroyTime; //파편 제거 시간
    public string strike_sound;
    public string destroy_sound;

    public void Mining()
    {
        SoundManager.Instance.PlaySE(strike_sound);
        var clone = Instantiate(rock_effect, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        --hp;
        if (hp <= 0) Destruction();
    }

    void Destruction()
    {
        SoundManager.Instance.PlaySE(destroy_sound);
        col.enabled = false;
        Destroy(rock);

        for (int i = 0; i < count; i++)
        {
            Instantiate(rock_Item_Prefab, rock.transform.position, Quaternion.identity);
        }

        debris.SetActive(true);
        Destroy(debris, destroyTime);
    }
}
