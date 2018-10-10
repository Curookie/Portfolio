using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownCtrl : MonoBehaviour {


    private int _dropdownIndex;
    private Dropdown _dd;

    private int _testamentCode =1;

    public GameObject[] _ddObjs;

    private void Awake()
    {
        _dropdownIndex = int.Parse(name.Substring(8, 1));
        _dd = GetComponent<Dropdown>();
        InitDropdown();
    }

    public void InitDropdown()
    {
        if (_dropdownIndex == 1)
        {
            _dd.options.Clear();
            for (int i = 0; i < 9; i++)
            {
                _dd.options.Add(new Dropdown.OptionData() { text = LevelManager._lm.GetAbilityName(i + 1) });
            }
            _dd.value = 0;
            _dd.RefreshShownValue();
        }
        else if (_dropdownIndex == 2)
        {
            ChangeTestaments(1);
        }
        else if (_dropdownIndex == 3)
        {
            ChangeChapters(1);
        }
    }

    public void ChangeTestaments(int AbilityCode)
    {
        _dd.options.Clear();
        List<int> tC = BibleManager._bm.GetTestamentsCodePerAbilityCode(AbilityCode);
        for (int i = tC[0]; i <= tC[tC.Count-1]; i++)
        {
            _dd.options.Add(new Dropdown.OptionData() { text = BibleManager._bm.GetTestamentName(i) } );
        }
        _dd.value = 0;
        _dd.RefreshShownValue();
    }

    public void ChangeChapters(int TestamentCode)
    {
        _dd.options.Clear();
        int lastChtCode = BibleManager._bm.GetTotalChapterNum(TestamentCode);
        for(int i=1; i<=lastChtCode; i ++)
        {
            _dd.options.Add(new Dropdown.OptionData() { text = i+"장" });
        }
        _dd.value = 0;
        _dd.RefreshShownValue();
    }
    
    public void ChangedValue()
    {
        if(_dropdownIndex==1)
        {
            if(_ddObjs[1].activeSelf)
            {
                _ddObjs[1].GetComponent<DropdownCtrl>().ChangeTestaments(_dd.value+1);
            }
            if(_ddObjs[2].activeSelf)
            {
                _ddObjs[1].GetComponent<DropdownCtrl>().SetDropdown2tmtCode(BibleManager._bm.GetFirstTestamentCodePerAbilityCode(_dd.value + 1));
                _ddObjs[2].GetComponent<DropdownCtrl>().ChangeChapters(_testamentCode);
            }
        } else if(_dropdownIndex ==2)
        {
            if (_ddObjs[2].activeSelf)
            {
                _ddObjs[2].GetComponent<DropdownCtrl>().ChangeChapters(_testamentCode + _dd.value);
            }
        }
    }
    
    public void SetDropdown2tmtCode(int TestamentCode)
    {
        _testamentCode = TestamentCode;
    }

}
