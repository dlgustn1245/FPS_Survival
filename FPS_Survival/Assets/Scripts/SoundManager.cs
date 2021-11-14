using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //소리의 이름
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    #region singleton
    public static SoundManager Instance
    {
        get
        {
            if (!instance) return null;
            return instance;
        }
    }
    static SoundManager instance;

    void Awake() //객체 생성치 최초 실행
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundNames;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    void Start()
    {
        playSoundNames = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundNames[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중");
                return;
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되지 않음");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundNames[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + name + "사운드가 없음");
    }
}
