using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    
    private static long _realHeart;
    private static long _realBibleEnergy;

    //Found VerseCard
    //private List<BibleCard> hasVerse = new List<BibleCard>();
    //private bool[] forChkVerse = new bool[31101];

    //Player Info
    private Text _heartText;
    private Text _bibleEnergyText;

    //Animation
    public GameObject _getHeartText;

    public static GameManager _gm = null;
    private LevelManager lm;
    private BibleManager bm;
    private BoxManager boxm;
    private AudioManager am;
    private DataManager dm;

    private void Awake()
    {

        if (_gm == null)
        {
            _gm = this;
        }
        else
        {
            RefreshRealHeart();
            RefreshRealBibleEnergy();
        }
    }
    
    private void Start()
    {
        dm = DataManager._dm;
        lm = LevelManager._lm;
        bm = BibleManager._bm;
        boxm = BoxManager._boxm;

        _getHeartText = Resources.Load("Prefabs/G_getHeart") as GameObject;
        _heartText = GameObject.FindGameObjectWithTag("HeartPoint").GetComponent<Text>();
        _bibleEnergyText = GameObject.FindGameObjectWithTag("BibleEnergy").GetComponent<Text>();

        LoadData();
        Debug.Log("s");
        
        RefreshRealHeart();
        RefreshRealBibleEnergy();

        StartCoroutine(GetAutoHeart());
    }

    private void LoadData()
    {
        _realHeart = dm.GetHeartPoint();
        _realBibleEnergy = dm.GetBibleEnergy();
    }

    public void RefreshRealBibleEnergy()
    {
        _bibleEnergyText.text = NumberManager.NtoS(_realBibleEnergy);
    }

    public void RefreshRealHeart()
    {
        _heartText.text = NumberManager.NtoS(_realHeart);
    }

    public void GotoEpisode(int num)
    {
        SceneManager.LoadScene("Episode"+num);
    }

    public long GetRealHeart()
    {
        return _realHeart;
    }

    public long GetRealBibleEnergy()
    {
        return _realBibleEnergy;
    }
    
    public void UseHeart(long v)
    {
        _realHeart -= v;
        dm.SetHeartPoint(_realHeart);
    }

    public void AddHeart(long v)
    {
        GameObject h = Instantiate(_getHeartText,_heartText.transform.parent.transform.parent.transform);
        h.transform.Find("Heart").GetComponent<Text>().text = "+"+NumberManager.NtoS(v);
        _realHeart += v;
        dm.SetHeartPoint(_realHeart);
        RefreshRealHeart();
    }

    public void AddBibleEnergy(long v)
    {
        _realBibleEnergy += v;
        dm.SetBibleEnergy(_realBibleEnergy);
        RefreshRealBibleEnergy();
    }

    IEnumerator GetAutoHeart()
    {
        while(true)
        {
            AddHeart(lm.GetHeartAutoGetValue());
            yield return new WaitForSeconds(lm.GetHeartAutoGetTime());
        }
    }

   
}
