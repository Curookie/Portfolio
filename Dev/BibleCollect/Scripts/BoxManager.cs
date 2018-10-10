using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class BoxManager : MonoBehaviour {

    public static BoxManager _boxm = null;

    public static int[] _boxCount = new int[] { 12, 9, 7, 10, 0 };

    private BibleManager bm;
    private FindManager fm;
    private AudioManager am;

    //G_popup1
    public GameObject G_popup1;
    public Text _Pup1_VerseTitle;
    public Text _Pup1_VerseText;
    public Text _Pup1_BibleEnergy;
    public RawImage _Pup1_AbilityIcon;
    public RawImage _Pup1_CardImage;
    public RawImage[] _Pup1_Stars;

    //G_popup2
    public GameObject G_popup2;
    public Text _Pup2_VerseTitle;
    public Text _Pup2_VerseText;
    public Text _Pup2_BibleEnergy;
    public RawImage _Pup2_AbilityIcon;
    public RawImage _Pup2_CardImage;
    public RawImage[] _Pup2_Stars;
    public Image _Pup2_BoxImage;
    public Text _Pup2_BoxCount;
    public GameObject _Pup2_AbiGroup;
    public Slider _Pup2_Slider;
    public GameObject G_BoxBtnList;
    public GameObject G_BoxCntList;

    public Sprite[] _boxSpriteList;

    public HorizontalScrollSnap _forChkNowGift;
    
    //G_popup3
    public GameObject G_popup3;
    public GameObject[] _Pup3_Dropdowns;
    public Text _Pup3_Title;
    private bool _isAllBoxOpen = false;

    private int _allOpenBoxCount;

    private int _selectedAbiCode = -1;
    private int _selectedTmtCode = -1;
    private int _selectedChtCode = -1;

    private float _timer = 0.0f;
    private bool _isOepnCard = false;

    private void Awake()
    {
        if (_boxm == null)
            _boxm = this;
    }

    private void Start()
    {
        bm = BibleManager._bm;
        fm = FindManager._fm;
        am = AudioManager._am;

        ChangeMenuUI();
    }

    public void OneBoxBtnPressed()
    {
        OpenOneBox(_forChkNowGift.CurrentPage);
    }

    public void AllBoxBtnPressed()
    {
        OpenAllBox(_forChkNowGift.CurrentPage);
    }

    public void MoveSelectMenu(int boxIndex)
    {
        string title="";
        if (boxIndex== 1)
        {
            title = "천사";
            _Pup3_Dropdowns[0].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[0].SetActive(true);
            _Pup3_Dropdowns[1].SetActive(false);
            _Pup3_Dropdowns[2].SetActive(false);
        } else if (boxIndex == 2)
        {
            title = "성서";
            _Pup3_Dropdowns[0].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[1].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[0].SetActive(true);
            _Pup3_Dropdowns[1].SetActive(true);
            _Pup3_Dropdowns[2].SetActive(false);
        } else if (boxIndex == 3)
        {
            title = "장";
            _Pup3_Dropdowns[0].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[1].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[2].GetComponent<DropdownCtrl>().InitDropdown();
            _Pup3_Dropdowns[0].SetActive(true);
            _Pup3_Dropdowns[1].SetActive(true);
            _Pup3_Dropdowns[2].SetActive(true);
        }

        _Pup3_Title.text = "원하는 " + title + " 선택";
        G_popup3.SetActive(true);
    }

    public void SeletedBoxOpen()
    {
        _selectedAbiCode = _Pup3_Dropdowns[0].GetComponent<Dropdown>().value + 1;

        if (_Pup3_Dropdowns[1].activeSelf)
        {
            _selectedTmtCode = bm.GetFirstTestamentCodePerAbilityCode(_selectedAbiCode) + _Pup3_Dropdowns[1].GetComponent<Dropdown>().value;
        }

        if (_Pup3_Dropdowns[2].activeSelf)
        {
            _selectedChtCode = bm.GetFirstChapterCodePerTestamentCode(_selectedTmtCode) + _Pup3_Dropdowns[2].GetComponent<Dropdown>().value;
        }

        G_popup3.SetActive(false);
        if (_isAllBoxOpen)
        {
            StartCoroutine(NormalBoxAllOpen());
            G_popup2.SetActive(true);
        } else
        {
            NormalBoxOpen();
            G_popup1.SetActive(true);
        }
            
    }

    public void StopAllOpenBtnPressed()
    {
        if(_isOepnCard)
        {
            _isOepnCard = false;
            _timer = 0.0f;
            _Pup2_Slider.GetComponent<Slider>().value = _timer;
            ChangeBtnUI();
        } else
        {
            G_popup2.SetActive(false);
            InitPopup2();
        }
    }

    public void ChangeBtnUI()
    {
        Image i = G_popup2.transform.Find("Panel/EndBtn").GetComponent<Image>();
        i.color = new Color32(0x1E, 0xA3, 0x51, 0xFF);
        G_popup2.transform.Find("Panel/EndBtn/Text").GetComponent<Text>().text = "모두 기록하기";


        //i.color = new Color32(0xC0, 0x38, 0x2E, 0xFF);
    }

    public void OpenOneBox(int _boxIndex)
    {
        if (_boxCount[_boxIndex] < 1) return;
        _boxCount[_boxIndex] -= 1;
        _isAllBoxOpen = false;

        switch (_boxIndex)
        {
            case 0:
                NormalBoxOpen();
                G_popup1.SetActive(true);
                break;
            case 1:
                MoveSelectMenu(_boxIndex);
                break;
            case 2:
                MoveSelectMenu(_boxIndex);
                break;
            case 3:
                MoveSelectMenu(_boxIndex);
                break;
            default:
                break;
        }
    }

    public void OpenAllBox(int _boxIndex)
    {
        if (_boxCount[_boxIndex] < 1) return;
        _allOpenBoxCount = _boxCount[_boxIndex];
        _isAllBoxOpen = true;

        switch (_boxIndex)
        {
            case 0:
                StartCoroutine(NormalBoxAllOpen());
                G_popup2.SetActive(true);
                break;
            case 1:
                MoveSelectMenu(_boxIndex);
                break;
            case 2:
                MoveSelectMenu(_boxIndex);
                break;
            case 3:
                MoveSelectMenu(_boxIndex);
                break;
            default:
                break;
        }
    }

    public void NormalBoxOpen()
    {

        for (int i = 0; i < 5; i++)
        {
            _Pup1_Stars[i].texture = Resources.Load("images/star_e") as Texture;
        }
        int verseCode;
        if (_selectedChtCode!=-1)
        {
            verseCode = Random.Range(bm.GetFirstVerseCodePerChapterCode(_selectedChtCode), bm.GetLastVerseCodePerChapterCode(_selectedChtCode)); 
        } else if (_selectedTmtCode != -1)
        {
            verseCode = Random.Range(bm.GetFirstVerseCodePerTestamentCode(_selectedTmtCode), bm.GetLastVerseCodePerTestamentCode(_selectedTmtCode));
        } else if (_selectedAbiCode != -1)
        {
            verseCode = Random.Range(bm.GetFirstVerseCodePerAbilityCode(_selectedAbiCode), bm.GetLastVerseCodePerAbilityCode(_selectedAbiCode));
        } else
        {
            verseCode = Random.Range(0, BibleManager._verseTotCount);
        }

        BibleData d = DataManager.bd[verseCode];
        _Pup1_VerseTitle.text = d.title;
        _Pup1_VerseText.text = d.text;

        int txtCntForBE = d.text.Replace(" ", "").Length;
        int rareRate = fm.SetNormalRareStyle();
        int testamentCode = bm.GetTestamentCode(verseCode);
        int abiCode = bm.GetAbilityCode(testamentCode);

        _Pup1_CardImage.texture = Resources.Load("images/card"+ rareRate) as Texture;
        _Pup1_AbilityIcon.texture = bm._abilityImages[2 * ( abiCode - 1)];
        for (int i = 0; i < rareRate; i++)

        {
            _Pup1_Stars[i].texture = Resources.Load("images/star") as Texture;
        }

        long BE = (txtCntForBE + Random.Range(0, 21)) * (30 / long.Parse(Mathf.Pow(2, (5 - rareRate)) + ""));
        _Pup1_BibleEnergy.text = BE +"";
        if (rareRate < 2)
            am.FindCardSoundPlay(0);
        else
            am.FindCardSoundPlay(1);

        BibleCard c = new BibleCard(verseCode, bm.GetChapterCode(verseCode), testamentCode, abiCode, BE, rareRate);

        bm.SetVerseToCht(c);
        ChangeMenuUI();
    }

    IEnumerator NormalBoxAllOpen()
    {
        int giftType = _forChkNowGift.CurrentPage;
        _Pup2_BoxImage.sprite = _boxSpriteList[giftType];
        _isOepnCard = true;

        for (int k =0; k< _allOpenBoxCount; k++)
        {
            _timer = 0.0f;
            for (int i = 0; i < 5; i++)
            {
                _Pup2_Stars[i].texture = Resources.Load("images/star_e") as Texture;
            }
            int verseCode;

            if (_selectedChtCode != -1)
            {
                verseCode = Random.Range(bm.GetFirstVerseCodePerChapterCode(_selectedChtCode), bm.GetLastVerseCodePerChapterCode(_selectedChtCode));
            }
            else if (_selectedTmtCode != -1)
            {
                verseCode = Random.Range(bm.GetFirstVerseCodePerTestamentCode(_selectedTmtCode), bm.GetLastVerseCodePerTestamentCode(_selectedTmtCode));
            }
            else if (_selectedAbiCode != -1)
            {
                verseCode = Random.Range(bm.GetFirstVerseCodePerAbilityCode(_selectedAbiCode), bm.GetLastVerseCodePerAbilityCode(_selectedAbiCode));
            }
            else
            {
                verseCode = Random.Range(0, BibleManager._verseTotCount);
            }

            BibleData d = DataManager.bd[verseCode];
            _Pup2_VerseTitle.text = d.title;
            _Pup2_VerseText.text = d.text;

            int txtCntForBE = d.text.Replace(" ", "").Length;
            int rareRate = fm.SetNormalRareStyle();
            int testamentCode = bm.GetTestamentCode(verseCode);
            int abiCode = bm.GetAbilityCode(testamentCode);

            _Pup2_CardImage.texture = Resources.Load("images/card" + rareRate) as Texture;
            _Pup2_AbilityIcon.texture = bm._abilityImages[2 * (abiCode - 1)];
            for (int i = 0; i < rareRate; i++)

            {
                _Pup2_Stars[i].texture = Resources.Load("images/star") as Texture;
            }

            long BE = (txtCntForBE + Random.Range(0, 21)) * (30 / long.Parse(Mathf.Pow(2, (5 - rareRate)) + ""));
            _Pup2_BibleEnergy.text = BE + "";
            if (rareRate < 2)
                am.FindCardSoundPlay(0);
            else
                am.FindCardSoundPlay(1);

            BibleCard c = new BibleCard(verseCode, bm.GetChapterCode(verseCode), testamentCode, abiCode, BE, rareRate);

            bm.SetVerseToCht(c);
            _boxCount[giftType] -= 1;
            Text s = _Pup2_AbiGroup.transform.Find("Image (" + (c.GetAbilityCode()-1) + ")/Text").GetComponent<Text>();
            s.text = (int.Parse(s.text)+1)+"";
            _Pup2_BoxCount.text = "남은 개수 : " + _boxCount[giftType];
            if (k == _allOpenBoxCount - 1) StopAllOpenBtnPressed();
            yield return new WaitForSeconds(2);
            if (!_isOepnCard || _timer < 2.0f)
            { _isOepnCard = false; break; }
        }
    }

    private void Update()
    {
        if (_isOepnCard && _timer <= 2.0f)
        {
            _timer += Time.deltaTime;
            _Pup2_Slider.GetComponent<Slider>().value = _timer;
        }
    }

    public void InitPopup2()
    {
        Image i = G_popup2.transform.Find("Panel/EndBtn").GetComponent<Image>();
        i.color = new Color32(0xC0, 0x38, 0x2E, 0xFF);
        G_popup2.transform.Find("Panel/EndBtn/Text").GetComponent<Text>().text = "그만 열기";
        for(int k=0; k<9; k++)
        {
            _Pup2_AbiGroup.transform.Find("Image (" + k + ")/Text").GetComponent<Text>().text = "0";
        }
        ChangeMenuUI();
    }

    public void ChangeMenuUI()
    {
        _selectedAbiCode = -1;
        _selectedChtCode = -1;
        _selectedTmtCode = -1;
        Color[] color = new Color[] { new Color32(0xDB, 0x28, 0x28, 0xFF), new Color32(0x54, 0x54, 0x54, 0xFF) };
        for (int i=0; i<_forChkNowGift.ChildObjects.Length; i++)
        {
            G_BoxBtnList.transform.Find("BoxBtn1 (" + i + ")/BG/Text").GetComponent<Text>().text = _boxCount[i]+"";
            G_BoxCntList.transform.Find("BoxList (" + i + ")/BG/Text").GetComponent<Text>().text = _boxCount[i] + "";
            Image btn = G_BoxBtnList.transform.Find("BoxBtn1 (" + i + ")/BG").GetComponent<Image>();
            Image cnt = G_BoxCntList.transform.Find("BoxList (" + i + ")/BG").GetComponent<Image>();
            if(_boxCount[i]==0)
            {
                btn.color = color[1];
                cnt.color = color[1];
            } else
            {
                btn.color = color[0];
                cnt.color = color[0];
            }
        }
    }
}
