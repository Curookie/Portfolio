using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] StageAttackList;
    public AudioClip[] FindCardList;
    public AudioClip[] UpgradeActionList;
    private AudioSource _as;
    public static AudioManager _am;

    private void Awake()
    {
        if (_am == null)
            _am = this;
        _as = GetComponent<AudioSource>();
    }
    public void StageAttackSoundPlay(int num)
    {
        _as.PlayOneShot(StageAttackList[num]);
    }

    public void FindCardSoundPlay(int isNormalOrRare)
    {
        _as.PlayOneShot(FindCardList[isNormalOrRare]);
    }

    public void UpdgradeActionPlay(int isSpecialOrNormal)
    {
        _as.PlayOneShot(UpgradeActionList[isSpecialOrNormal]);
    }
}
