using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindManager : MonoBehaviour
{

    //Finding Card
    private int _touchCount;
    private static int _findVerseCode;
    private int _findTestamentCode = 0;
    private int _findAbilityCode = 0;
    private string _findBibleRawText;
    private string _findBibleRawTitle;

    public Text _findBibleText;
    public Text _findBibleTitle;
    public Text _findTouchNeed;
    public Text _findBibleEnergy;

    //Images
    public RawImage _cardImage;
    public RawImage[] _starImage;
    public RawImage _bibleImage;

    public static FindManager _fm;
    private GameManager gm;
    private BoxManager boxm;
    private BibleManager bm;
    private LevelManager lm;
    private AudioManager am;

    private void Awake()
    {
        if (_fm == null)
            _fm = this;
    }

    private void Start()
    {
        lm = LevelManager._lm;
        bm = BibleManager._bm;
        am = AudioManager._am;
        gm = GameManager._gm;

        ChooseBible();
    }

    public void SkipBible()
    {
        ChooseBible();
    }

    void ChooseBible()
    {
        int i = Random.Range(0, BibleManager._verseTotCount);

        _cardImage.texture = Resources.Load("Images/card_e") as Texture;
        _findBibleEnergy.gameObject.SetActive(false);

        for (int j = 0; j < 5; j++)
        {
            if (_starImage[j].texture.name == "star")
                _starImage[j].texture = Resources.Load("Images/star_e") as Texture;
        }
        _touchCount = 0;
        _findBibleText.text = "";
        _findBibleTitle.text = "???";
        _findBibleRawText = DataManager.bd[i].text;
        _findBibleRawTitle = DataManager.bd[i].title;
        _findTouchNeed.text = _findBibleRawText.Replace(" ", "").Length.ToString();
        //Debug.Log(bd[i].text.Replace(" ","").ToString());
        _findVerseCode = i;
        //Debug.Log("Selected Bible: " + _findBibleCode);
        _findTestamentCode = bm.GetTestamentCode(i);
        _findAbilityCode = bm.GetAbilityCode(_findTestamentCode);
        _bibleImage.GetComponent<RawImage>().texture = bm._abilityImages[2 * _findAbilityCode - 1];
        return;
    }

    public void TouchBible()
    {
        long totHaert = 0;
        while (true)
        {
            if (_findBibleRawText.Length == _findBibleText.text.Length)
            {
                SkipBible();
                break;
            }
            if (_findBibleRawText[_findBibleText.text.Length].Equals(' '))
            {
                _findBibleText.text += " ";
            }
            else
            {
                _findBibleText.text += _findBibleRawText[_findBibleText.text.Length];
                _touchCount += 1;
                _findTouchNeed.text = (int.Parse(_findTouchNeed.text) - 1).ToString();
                if (int.Parse(_findTouchNeed.text) == 0)
                {
                    int n = SetNormalRareStyle();
                    _cardImage.texture = Resources.Load("Images/card" + n) as Texture;

                    for (int j = 0; j < n; j++)
                    {
                        _starImage[j].texture = Resources.Load("Images/star") as Texture;
                    }

                    //FX Select AND Play
                    if (n == 1)
                        am.FindCardSoundPlay(0);
                    else
                        am.FindCardSoundPlay(1);

                    //Show Card's Title, BE, Ability 
                    _findBibleTitle.text = _findBibleRawTitle;
                    _findBibleEnergy.gameObject.SetActive(true);
                    _bibleImage.GetComponent<RawImage>().texture = bm._abilityImages[2 * (_findAbilityCode - 1)];

                    //Card's BE Creating
                    long cardBE = (_touchCount + Random.Range(0, 21)) * (30 / long.Parse(Mathf.Pow(2, (5 - n)) + ""));
                    _findBibleEnergy.text = "+" + cardBE;

                    //Making VerseCard
                    BibleCard findCard = new BibleCard(_findVerseCode, bm.GetChapterCode(_findVerseCode), _findTestamentCode, _findAbilityCode, cardBE, n);
                    //Add VerseCard and PerfationInfo
                    bm.SetVerseToCht(findCard);
                    //Add UI VerseCard
                    //GameObject addCard = Instantiate(verseminiCard[n - 1], verseUI.transform);
                    //addCard.transform.Find("Text").GetComponent<Text>().text = findBibleRawTitle;

                    //Add Heart == Card's Text's Count 
                    totHaert += _touchCount;
                    //for Touching Process End
                    break;
                }
                //for Show Texts Counts per TouchLv
                if (_touchCount % lm.GetTouchLvValue() == 0) break;
            }
        }
        totHaert += lm.GetHeartGetValue();
        //for Touch Heart
        gm.AddHeart(totHaert);
    }

    public int SetNormalRareStyle()
    {
        int n = 1;
        float i = Random.Range(0.0f, 100.0f);

        if (i < 60.0f) n = 1;
        else if (i < 80.0f) n = 2;
        else if (i < 90.0f) n = 3;
        else if (i < 95.0f) n = 4;
        else if (i < 100.0f) n = 5;

        return n;
    }
}
