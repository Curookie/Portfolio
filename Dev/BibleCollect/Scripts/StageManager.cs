using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {

    private int _Stage;
    private GameObject _Map;
    public GameObject[] EnemyType;
    public GameObject _stageName;
    public GameObject[] _abilityObj;
    private int _stageEnemyCount = 0;
    private int _abilityEndCount = 0;

    public static StageManager _SMInstance = null;

    private readonly Dictionary<string, Dictionary<string, int>> StageEnemyCtrl = new Dictionary<string, Dictionary<string, int>> {
        { "Stage1", new Dictionary<string, int> {
            { "Type01", 2 }
        } },
        { "Stage2", new Dictionary<string, int> {
            { "Type01", 3 }
        } },
        { "Stage3", new Dictionary<string, int> {
            { "Type01", 2 },
            { "Type02", 1 }
        } },
        { "Stage4", new Dictionary<string, int> {
            { "Type01", 3 },
            { "Type02", 1 }
        } },
        { "Stage5", new Dictionary<string, int> {
            { "Type01", 2 },
            { "Type02", 2 }
        } },
        { "Stage6", new Dictionary<string, int> {
            { "Type01", 4 },
            { "Type02", 2 }
        } },
        { "Stage7", new Dictionary<string, int> {
            { "Type01", 3 },
            { "Type02", 3 }
        } },
        { "Stage8", new Dictionary<string, int> {
            { "Type01", 2 },
            { "Type02", 4 }
        } },
        { "Stage9", new Dictionary<string, int> {
            { "Type02", 5 }
        } },
        { "Stage10", new Dictionary<string, int> {
            { "Type01", 2 },
            { "Type02", 3 },
            { "Type03", 1 }
        } }
    };
   
    private void Awake()
    {
        if (_SMInstance == null)
            _SMInstance = this;
        //else if (_SMInstance != this)
        //  Destroy(gameObject);

        InitStage();
    }

    private void Start()
    {
        _Map = GameObject.FindGameObjectWithTag("Map");
        _stageName.SetActive(true);
        GenerateStage(1);
        _stageName.GetComponent<Animator>().SetTrigger("StageNameShow");
        StartCoroutine(CheckLevel());
        Debug.Log(BoxManager._boxCount[0]+ " "+ BoxManager._boxCount[1] + " " + BoxManager._boxCount[2] + " " + BoxManager._boxCount[3]);
    }

    public int GetEnemyCount()
    {
        return _stageEnemyCount;
    }

    IEnumerator CheckLevel()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            Debug.Log(_stageEnemyCount+" ,"+_abilityEndCount);
            if(_stageEnemyCount==0)
            {
                NextStage();
            }
            else if(_abilityEndCount>=9)
            {
                Invoke("StageEnd", 5);
            }
        }
    }

    public void EnemyDie()
    {
        _stageEnemyCount -= 1;
    }

    public void AbilityEnd()
    {
        _abilityEndCount += 1;
    }

    public void InitStage()
    {
        int s = int.Parse(Application.loadedLevelName.Substring(7));
        if(s == 6)
        {
            _Stage = 50;
            return;
        }
        _Stage = (s - 1) * 10 + 1;
    }
    
    public int GetStage()
    {
        return _Stage;
    }

    public void NextStage()
    {
        _Stage += 1;
        GenerateStage(_Stage);
        _stageName.transform.Find("Text").GetComponent<Text>().text = "Stage "+_Stage;
        _stageName.GetComponent<Animator>().SetTrigger("StageNameShow");
        //ability if reset abilitycount-> 0 reset also
        _abilityEndCount = 0;
        foreach(GameObject a in _abilityObj)
        {
            a.GetComponent<AbilityCtrl>().StageReseted();
        }
    }

    public void GenerateStage(int Stage)
    {
        Dictionary<string, int> sG = StageEnemyCtrl["Stage" + Stage];
        for (int i = 1; i <= sG.Count; i++)
        {
            for (int j = 0; j < sG["Type" + string.Format("{0:00}",i)]; j++)
            {
                float x = Random.Range(-7.0f, 7.0f);
                float y = Random.Range(-4.8f, 9.0f);
                Instantiate(EnemyType[i-1], new Vector2(x,y), Quaternion.identity, _Map.transform);
                _stageEnemyCount++;
            }
        }
    }

    public void StageEnd()
    {
        SceneManager.LoadScene("Game");
    }
}
