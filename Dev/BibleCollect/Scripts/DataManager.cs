using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class BibleData
{
    public string title;
    public string text;
}

[Serializable]
public class MainData
{
    //Main Data
    public long heartPoint;
    public long bibleEnergy;
}

[Serializable]
public class VerseData
{
    //Bible Data
    public int onlyVerseCount;
    public int[] onlyVerseCountPerAbility;
    public long hasTotalVerseCount;
    public long[] bibleEnergyPerAbility;
    public long[] bibleEnergyPerChapter;
    public bool[] forCheckVerseTable;
}

[Serializable]
public class OverlapData
{
    //OverlapData
    public Dictionary<int, int> verseCodeAndOverlapCount;
    public Dictionary<int, int> verseCodeAndBestRank;
}


[Serializable]
public class CardListPerChapter
{
    //CardsGroup

    public Dictionary<string, string> allVerseJsonPerChapter;
}

[Serializable]
public class CardList
{
    public List<BibleCard>[] verseList;
}

[Serializable]
public class LevelData
{
    //Level Data
    public int touchLv;
    public int heartGetPerTouchLv;
    public int heartAutoGetPerTimeLv;
    public int bibleAutoGetPerTimeLv;
    public int[] abilityLv;
}

public class DataManager : MonoBehaviour
{

    //Total Verse Contents
    public static BibleData[] bd = null;

    private MainData _MD;
    private string _mainJson;
    private VerseData _VD;
    private string _verseJson;
    private OverlapData _OD;
    private string _overlapJson;
    private CardList _CL;
    private CardListPerChapter _CLC;
    private string _cardJson;

    private LevelData _LD ;
    private string _levelJson;

    //private string _verseCodeAndOverlapCountJson;
    //private string _verseCodeAndBestRankJson;
    private string[] _verseJsonListPerCht = Enumerable.Repeat("", 1189).ToArray();
    //private Dictionary<string, string> _verseJsonDicPerAbi = new Dictionary<string, string>();

    public static DataManager _dm;

    private void Awake()
    {
        if (_dm == null)
        {
            _dm = this;
            DontDestroyOnLoad(_dm);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerPrefs.DeleteAll();
        LoadAllJsonFromPP();
    }

    public void LoadAllJsonFromPP()
    {
        ReadBible("kor_bible_json");
        _MD = new MainData();
        _VD = new VerseData();
        _OD = new OverlapData();
        _CL = new CardList();
        _CLC = new CardListPerChapter();
        _LD = new LevelData();

        if (!PlayerPrefs.HasKey("MainData")) InitAllJsonToPP();
        _mainJson = PlayerPrefs.GetString("MainData");
        _verseJson = PlayerPrefs.GetString("VerseData");
        _overlapJson = PlayerPrefs.GetString("OverlapData");
        _cardJson = PlayerPrefs.GetString("CardData");
        _levelJson = PlayerPrefs.GetString("LevelData");

        _CLC.allVerseJsonPerChapter = new Dictionary<string, string>();
        _CL.verseList = Enumerable.Repeat<List<BibleCard>>(null, 1189).ToArray();
        _OD.verseCodeAndOverlapCount = new Dictionary<int, int>();
        _OD.verseCodeAndBestRank = new Dictionary<int, int>();
        for (int i = 0; i < 1189; i++)
        {
            _verseJsonListPerCht[i] = JsonHelper.FromJson<string>(_cardJson)[i];
            var a = JsonUtility.FromJson<Serialization<string, string>>(_verseJsonListPerCht[i]).ToDictionary();
            _CLC.allVerseJsonPerChapter.Add(a.Keys.ElementAt(0), a.Values.ElementAt(0));
            _CL.verseList[i] = JsonUtility.FromJson<Serialization<BibleCard>>(_CLC.allVerseJsonPerChapter["cht" + (i+1)]).ToList();
        }

        //Debug.Log(JsonHelper.FromJson<string>(_overlapJson)[1]);
        
        _OD.verseCodeAndOverlapCount = JsonUtility.FromJson<Serialization<int,int>> ( JsonHelper.FromJson<string>(_overlapJson)[0]).ToDictionary();
        _OD.verseCodeAndBestRank = JsonUtility.FromJson<Serialization<int, int>>( JsonHelper.FromJson<string>(_overlapJson)[1]).ToDictionary();


    }

    private void InitAllJsonToPP()
    {
        _MD.heartPoint = 0L;
        _MD.bibleEnergy = 0L;

        _VD.onlyVerseCount = 0;
        _VD.hasTotalVerseCount = 0L;
        _VD.onlyVerseCountPerAbility = Enumerable.Repeat(0, 9).ToArray();
        _VD.bibleEnergyPerAbility = Enumerable.Repeat(0L, 9).ToArray();
        _VD.bibleEnergyPerChapter = Enumerable.Repeat(0L, 1189).ToArray();
        _VD.forCheckVerseTable = Enumerable.Repeat(false, 31101).ToArray();

        string stringData3 = "";
        string[] group = new string[2];
        _OD.verseCodeAndOverlapCount = new Dictionary<int, int>();
        _OD.verseCodeAndBestRank = new Dictionary<int, int>();
        var tempData = _OD.verseCodeAndOverlapCount;
        group[0] = JsonUtility.ToJson(new Serialization<int, int>(tempData));
        tempData = _OD.verseCodeAndBestRank;
        group[1] = JsonUtility.ToJson(new Serialization<int, int>(tempData));
        stringData3 = JsonHelper.ToJson(group);

        string[] cards = new string[1189];

        CardList tempData2 = new CardList();
        tempData2.verseList = Enumerable.Repeat<List<BibleCard>>(null, 1189).ToArray();

        for (int i = 0; i < 1189; i++)
        {
            string tempString = JsonUtility.ToJson(new Serialization<BibleCard>(tempData2.verseList[i]));
            //Debug.Log(tempString);
            _CLC.allVerseJsonPerChapter = new Dictionary<string, string> { { "cht"+(i+1), tempString } };
            cards[i] = JsonUtility.ToJson(new Serialization<string, string>( _CLC.allVerseJsonPerChapter));
        }

        string stringData4 = JsonHelper.ToJson(cards);
        //Debug.Log(stringData4);

        _LD.touchLv = 1;
        _LD.heartGetPerTouchLv = 1;
        _LD.heartAutoGetPerTimeLv = 1;
        _LD.bibleAutoGetPerTimeLv = 0;
        _LD.abilityLv = Enumerable.Repeat(1, 9).ToArray();

        PlayerPrefs.SetString("MainData", JsonUtility.ToJson(_MD));
        PlayerPrefs.SetString("VerseData", JsonUtility.ToJson(_VD));
        PlayerPrefs.SetString("OverlapData", stringData3);
        PlayerPrefs.SetString("CardData", stringData4);
        PlayerPrefs.SetString("LevelData", JsonUtility.ToJson(_LD));
    }

    //init Functions
    private void ReadBible(string filepath)
    {
        TextAsset textAsset = Resources.Load(filepath) as TextAsset;
        bd = JsonHelper.FromJson<BibleData>(textAsset.text);
        return;
    }

    public void SetHeartPoint(long heartPoint)
    {
        string Or = @"{""heartPoint"": " + heartPoint+"}";
        JsonUtility.FromJsonOverwrite(Or, _MD);
        _mainJson = JsonUtility.ToJson(_MD);
        SaveJsonToPP(_mainJson, 1);
        //Debug.Log(PlayerPrefs.GetString("MainData"));
    }

    public void SetBibleEnergy(long bibleEnergy)
    {
        string Or = @"{""bibleEnergy"": " + bibleEnergy + "}";
        JsonUtility.FromJsonOverwrite(Or, _MD);
        _mainJson = JsonUtility.ToJson(_MD);
        SaveJsonToPP(_mainJson, 1);
        //Debug.Log(PlayerPrefs.GetString("MainData"));
    }

    public void SetTouchLv(int AddLv)
    {
        _LD.touchLv += AddLv;
        _levelJson = JsonUtility.ToJson(_LD);
        SaveJsonToPP(_levelJson, 5);
    }

    public void SetHeartGetPerTouchLv(int AddLv)
    {
        _LD.heartGetPerTouchLv += AddLv;
        _levelJson = JsonUtility.ToJson(_LD);
        SaveJsonToPP(_levelJson, 5);
    }

    public void SetHeartAutoGetPerTimeLv(int AddLv)
    {
        _LD.heartAutoGetPerTimeLv += AddLv;
        _levelJson = JsonUtility.ToJson(_LD);
        SaveJsonToPP(_levelJson, 5);
    }

    public void SetBibleAutoGetPerTimeLv(int AddLv)
    {
        _LD.bibleAutoGetPerTimeLv += AddLv;
        _levelJson = JsonUtility.ToJson(_LD);
        SaveJsonToPP(_levelJson, 5);
    }

    public void SetAbilityLv(int AbilityCode, int AddLv)
    {
        _LD.abilityLv[AbilityCode] += AddLv;
        _levelJson = JsonUtility.ToJson(_LD);
        SaveJsonToPP(_levelJson, 5);
    }

    public long GetHeartPoint()
    {
        return _MD.heartPoint;
    }

    public long GetBibleEnergy()
    {
        return _MD.bibleEnergy;
    }

    public bool GetForCheckVerseTable(int VerseCode)
    {
        return _VD.forCheckVerseTable[VerseCode];
    }

    public long GetBibleEnergyPerAbility(int AbilityCode)
    {
        return _VD.bibleEnergyPerAbility[AbilityCode - 1];
    }

    public int GetOnlyVerseCount()
    {
        return _VD.onlyVerseCount;
    }

    public int GetOnlyVerseCountPerAbility(int AbilityCode)
    {
        return _VD.onlyVerseCountPerAbility[ AbilityCode - 1 ];
    }

    public int GetTouchLv()
    {
        return _LD.touchLv;
    }

    public int GetHeartGetPerTouchLv()
    {
        return _LD.heartGetPerTouchLv;
    }

    public int GetHeartAutoGetPerTimeLv()
    {
        return _LD.heartAutoGetPerTimeLv;
    }
    
    public int GetBibleAutoGetPerTimeLv()
    {
        return _LD.bibleAutoGetPerTimeLv;
    }

    public int GetAbilityLv(int AbilityCode)
    {
        return _LD.abilityLv[AbilityCode - 1];
    }

    public Dictionary<int, int> GetVerseCodeAndOverlapCount()
    {
        return _OD.verseCodeAndOverlapCount;
    }

    public Dictionary<int, int> GetVerseCodeAndBestRank()
    {
        return _OD.verseCodeAndBestRank;
    }

    public List<BibleCard> GetVerseList(int ChapterCode)
    {
        return _CL.verseList[ChapterCode];
    }

    public void PutForCheckVerseTable(int VerseCode, bool TorF)
    {
        _VD.forCheckVerseTable[VerseCode] = TorF;
    }

    public void PutBibleEnergyPerAbility(int AbilityCode, long AddBE)
    {
        _VD.bibleEnergyPerAbility[AbilityCode - 1] += AddBE;
    }

    public void PutBibleEnergyPerChapter(int ChapterCode, long AddBE)
    {
        _VD.bibleEnergyPerChapter[ChapterCode - 1] += AddBE;
    }

    public void PutOnlyVerseCount(int AddCount)
    {
        _VD.onlyVerseCount += AddCount;
    }

    public void PutOnlyVerseCountPerAbility(int AbilityCode, int AddCount)
    {
        _VD.onlyVerseCountPerAbility[AbilityCode - 1] += AddCount;
    }
    
    public void PutHasTotalVerseCount(long AddCount)
    {
        _VD.hasTotalVerseCount += AddCount;
    }

    public void SaveVD()
    {
        SaveJsonToPP(JsonUtility.ToJson(_VD),2);
    }

    public void SaveOD()
    {
        SaveJsonToPP(JsonHelper.ToJson(new string[] { JsonUtility.ToJson(new Serialization<int, int>(_OD.verseCodeAndOverlapCount)), JsonUtility.ToJson(new Serialization<int, int>(_OD.verseCodeAndBestRank)) }), 3);

        //Debug.Log(PlayerPrefs.GetString("OverlapData"));
    }

    public void SaveCD()
    {
        string[] cardsPerAbi = new string[1189];
        for (int i = 0; i < 1189; i++)
        {
            cardsPerAbi[i] = JsonUtility.ToJson(new Serialization<string, string>( new Dictionary<string, string>() { { "cht" + (i + 1), JsonUtility.ToJson(new Serialization<BibleCard>(_CL.verseList[i])) } } ));
        }
        SaveJsonToPP(JsonHelper.ToJson(cardsPerAbi), 4);

        //Debug.Log(PlayerPrefs.GetString("CardData"));
    }

    public void SaveJsonToPP(string jsonData, int dataType)
    {
        switch (dataType)
        {
            case 1:
                PlayerPrefs.SetString("MainData", jsonData);
                break;
            case 2:
                PlayerPrefs.SetString("VerseData", jsonData);
                break;
            case 3:
                PlayerPrefs.SetString("OverlapData", jsonData);
                break;
            case 4:
                PlayerPrefs.SetString("CardData", jsonData);
                break;
            case 5:
                PlayerPrefs.SetString("LevelData", jsonData);
                break;
            default:
                break;
        }
    }


    /*
    public void Save(SaveData s)
    {
        var b = new BinaryFormatter();
        var m = new MemoryStream();

        b.Serialize(m, s);
        PlayerPrefs.SetString("GameData", Convert.ToBase64String(m.GetBuffer()));
    }

    public bool Load(SaveData s)
    {
        var data = PlayerPrefs.GetString("GameData");
        if(!string.IsNullOrEmpty(data))
        {
            var b = new BinaryFormatter();
            var m = new MemoryStream();

            s = (SaveData) b.Deserialize(m);
            return true;
        } else
        {
            return false;
        }
    }*/
}
