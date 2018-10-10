using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private readonly string[] _abilityName = new string[] { "사랑이", "충성이", "기쁨이", "평화군", "인내양", "자비스", "선행녀", "온유", "절제남" };

    public static LevelManager _lm = null;

    private DataManager dm;
    private BibleManager bm;
    private GameManager gm;
    private BoxManager boxm;
    private AudioManager am;

    //Upgrade
    public GameObject[] _upgradeBtnUI;

    //InfoUI
    public GameObject _abilityInfoUI;
    
    void Awake()
    {
        if(_lm == null)
            _lm = this;

    }

    private void OnEnable()
    {
        dm = DataManager._dm;
        gm = GameManager._gm;
        am = AudioManager._am;
    }

    void Start()
    {
        bm = BibleManager._bm;
        InitUpgradeBtnUI();
        RefreshAbilityLvUI();
    }

    //For Button Init
    void InitUpgradeBtnUI()
    {
        _upgradeBtnUI[0].transform.Find("Upgrade1Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextTouchLvNeed(dm.GetTouchLv())) + "";
        _upgradeBtnUI[0].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetTouchLv() + "";
        _upgradeBtnUI[0].transform.Find("Info").GetComponent<Text>().text = "+" + GetTouchLvValue() + "글자 /";

        _upgradeBtnUI[1].transform.Find("Upgrade2Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextHeartGetLvNeed(dm.GetHeartGetPerTouchLv())) + "";
        _upgradeBtnUI[1].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetHeartGetPerTouchLv() + "";
        _upgradeBtnUI[1].transform.Find("Info").GetComponent<Text>().text = "+" + NumberManager.NtoS(GetHeartGetValue()) + " /";

        _upgradeBtnUI[2].transform.Find("Upgrade3Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextHeartAutoGetLvNeed(dm.GetHeartAutoGetPerTimeLv())) + "";
        _upgradeBtnUI[2].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetHeartAutoGetPerTimeLv() + "";
        _upgradeBtnUI[2].transform.Find("Info").GetComponent<Text>().text = "+" + NumberManager.NtoS(GetHeartAutoGetValue()) + " /";
    }

    //for Exp per Abi
    public int CalAbiExp(int AbilityCode, int AbilityLv)
    {
        int ExpVerse = 0;
        int TotExpVerse = bm.GetTotalVerseNumPerAbility(AbilityCode);

        for (int i = 1; i <= AbilityLv; i++)
        {
            if (AbilityLv >= 5) return 0;
            else if (i == 4) return TotExpVerse;
            ExpVerse += int.Parse(Math.Round(TotExpVerse * i * 0.1, 0)+"");
            //Debug.Log(ExpVerse);
        }
        return ExpVerse;
    }

    public string GetAbilityName(int AbilityCode)
    {
        return _abilityName[AbilityCode - 1];
    }

    public void AbilityLevelUp(int AbilityCode)
    {
        if (dm.GetAbilityLv(AbilityCode) >= 5)
            return;
        dm.SetAbilityLv(AbilityCode, 1);
        RefreshAbilityLvUI();
        return;
    }
    
    //Refresh AbilityInfoUI
    public void RefreshAbilityLvUI()
    {
        int v = bm.GetHasOnlyVerseNumPerAbility(BibleManager._nowOpenAbility);
        int m = CalAbiExp(BibleManager._nowOpenAbility, dm.GetAbilityLv(BibleManager._nowOpenAbility));
        int min = CalAbiExp(BibleManager._nowOpenAbility, dm.GetAbilityLv(BibleManager._nowOpenAbility) - 1);
        if (bm.GetHasOnlyVerseNumPerAbility(BibleManager._nowOpenAbility) >= m)
        {
            AbilityLevelUp(BibleManager._nowOpenAbility);
            min = CalAbiExp(BibleManager._nowOpenAbility, dm.GetAbilityLv(BibleManager._nowOpenAbility) - 1);
            _abilityInfoUI.transform.Find("Slider").GetComponent<Slider>().minValue = min;
            m = CalAbiExp(BibleManager._nowOpenAbility, dm.GetAbilityLv(BibleManager._nowOpenAbility));
        }
        if (dm.GetAbilityLv(BibleManager._nowOpenAbility) == 5)
        {
            v = bm.GetTotalVerseNumPerAbility(BibleManager._nowOpenAbility);
            m = bm.GetTotalVerseNumPerAbility(BibleManager._nowOpenAbility);
            min = 0;
        }
        //Debug.Log("v:" + v + "m:" + m + "min:" + min + "nowOpenAbility" + BibleManager.nowOpenAbility);
        _abilityInfoUI.transform.Find("Text").GetComponent<Text>().text = GetAbilityName(BibleManager._nowOpenAbility);
        _abilityInfoUI.transform.Find("Slider").GetComponent<Slider>().maxValue = m;
        _abilityInfoUI.transform.Find("Slider").GetComponent<Slider>().minValue = min;
        _abilityInfoUI.transform.Find("Slider").GetComponent<Slider>().value = v;
        _abilityInfoUI.transform.Find("Slider/Lv").GetComponent<Text>().text = "Lv. " + dm.GetAbilityLv(BibleManager._nowOpenAbility);
        _abilityInfoUI.transform.Find("Slider/Background/Info").GetComponent<Text>().text = v + " / " + m;
        _abilityInfoUI.transform.Find("Slider/Percent").GetComponent<Text>().text = string.Format("{0:0.00}", double.Parse((v - min) + "") / (m - min) * 100) + "%";
    }

    //Upgrade Functions
    public void TouchUpgrade()
    {
        if (gm.GetRealHeart() >= GetNextTouchLvNeed(dm.GetTouchLv()))
        {
            gm.UseHeart(GetNextTouchLvNeed(dm.GetTouchLv()));
            gm.RefreshRealHeart();
            dm.SetTouchLv(1);
            _upgradeBtnUI[0].transform.Find("Upgrade1Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextTouchLvNeed(dm.GetTouchLv())) + "";
            _upgradeBtnUI[0].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetTouchLv() + "";
            _upgradeBtnUI[0].transform.Find("Info").GetComponent<Text>().text = "+" + GetTouchLvValue() + "글자 /";
            am.UpdgradeActionPlay(0);
        }

    }

    public void HeartGetUpgrade()
    {
        if (gm.GetRealHeart() >= GetNextHeartGetLvNeed(dm.GetHeartGetPerTouchLv()))
        {
            gm.UseHeart(GetNextHeartGetLvNeed(dm.GetHeartGetPerTouchLv()));
            gm.RefreshRealHeart();
            dm.SetHeartGetPerTouchLv(1);
            _upgradeBtnUI[1].transform.Find("Upgrade2Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextHeartGetLvNeed(dm.GetHeartGetPerTouchLv())) + "";
            _upgradeBtnUI[1].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetHeartGetPerTouchLv() + "";
            _upgradeBtnUI[1].transform.Find("Info").GetComponent<Text>().text = "+" + NumberManager.NtoS(GetHeartGetValue()) + " /";
            am.UpdgradeActionPlay(0);
        }
    }

    public void HeartAutoGetUpgrade()
    {
        if (gm.GetRealHeart() >= GetNextHeartAutoGetLvNeed(dm.GetHeartAutoGetPerTimeLv()))
        {
            gm.UseHeart(GetNextHeartAutoGetLvNeed(dm.GetHeartAutoGetPerTimeLv()));
            gm.RefreshRealHeart();
            dm.SetHeartAutoGetPerTimeLv(1);
            _upgradeBtnUI[2].transform.Find("Upgrade3Btn/Need").GetComponent<Text>().text = NumberManager.NtoS(GetNextHeartAutoGetLvNeed(dm.GetHeartAutoGetPerTimeLv())) + "";
            _upgradeBtnUI[2].transform.Find("Lv").GetComponent<Text>().text = "Lv. " + dm.GetHeartAutoGetPerTimeLv() + "";
            _upgradeBtnUI[2].transform.Find("Info").GetComponent<Text>().text = "+" + NumberManager.NtoS(GetHeartAutoGetValue()) + " /";
            if (dm.GetHeartAutoGetPerTimeLv() % 10 == 0)
                _upgradeBtnUI[2].transform.Find("subInfo").GetComponent<Text>().text = GetHeartAutoGetTime() + "초";
            am.UpdgradeActionPlay(0);
        }
    }

    //Getter

    //Setter

    public long GetNextTouchLvNeed(int l)
    {
        return l * 20;
    }
    public long GetNextHeartGetLvNeed(int l)
    {
        return l * 20;
    }
    public long GetNextHeartAutoGetLvNeed(int l)
    {
        return l * 20;
    }
    public long GetNextBibleAutoGetLvNeed(int l)
    {
        return l * 20;
    }

    public long GetHeartGetValue()
    {
        return (long) Math.Pow(2, dm.GetHeartGetPerTouchLv()-1);
    }

    public int GetTouchLvValue()
    {
        return dm.GetTouchLv();
    }

    public long GetHeartAutoGetValue()
    {
        return long.Parse(100 + 20 * ((dm.GetHeartAutoGetPerTimeLv()+10)/10L) * (dm.GetHeartAutoGetPerTimeLv() - 1) + "");
    }

    public float GetHeartAutoGetTime()
    {
        return 20.0f-(dm.GetHeartAutoGetPerTimeLv()/10);
    }

}
