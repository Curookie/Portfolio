using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour {

	private long _attackEnergy;
    private GameObject _targetObject;
    private GameObject _damageUI;
    private Camera _mainCamera;

    private int _attackType;
    private int _strongTarget;

    private bool _isArrivedTarget = false;
    private int _chainAttackCount = 0;
    private long _chainDecreaseDamage;

    private float _attackSpeed = 5.0f;

    public GameObject[] damageType;

    private void Awake()
    {
        _attackType = int.Parse(name.Substring(10, 2));
        _damageUI = GameObject.FindGameObjectWithTag("DamageUI");
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        if (_attackType == 3)
        {
            int i = Random.Range(1, 9);
            if (i == 3) i = 9;
            _strongTarget = i;
            SelectTargeting(_strongTarget);
            StartCoroutine(Following());
        } else if (_attackType == 7)
        {
            Invoke("Delete", 5.0f);
        } else if (_attackType == 8 )
        {
            _attackEnergy *= 2;
            _chainDecreaseDamage = System.Convert.ToInt64(_attackEnergy * 0.125);
            if (Targeting())
            {
                StartCoroutine(Chaining());
            } else
            {
                Destroy(gameObject);
            }
        }  else if (_attackType == 9)
        {
            _attackSpeed = 10.0f;
            if (Targeting())
            {
                StartCoroutine(Following());
            }
            else
            {
                Destroy(gameObject);
            }
        } else
        {
            if (Targeting())
            {
                StartCoroutine(Following());
            } else
            {
                Destroy(gameObject);
            }
        }
    }
    
    public int GetChainCount()
    {
        return _chainAttackCount;
    }

    public void ChainArrived()
    {
        if (_chainAttackCount >= 5)
            Destroy(gameObject);

        _attackEnergy -= _chainDecreaseDamage;
        _chainAttackCount++;
        _targetObject = null;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public int GetAttackType()
    {
        return _attackType;
    }

    private IEnumerator Following()
    {
        while(true)
        {
            if (_targetObject != null)
                transform.position = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.deltaTime * _attackSpeed);
            else if (Targeting())
            {
                transform.position = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.deltaTime * _attackSpeed);
            }
            else if (StageManager._SMInstance.GetEnemyCount()==0)
            {
                //StopCoroutine(Following());
                Destroy(gameObject);
                //StageManager._SMInstance.NextStage();
                //break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Chaining()
    {
        while (true)
        {
            if (_targetObject != null)
                transform.position = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.deltaTime * 7f);
            else if (chainTargeting())
            {
                transform.position = Vector2.Lerp(transform.position, _targetObject.transform.position, Time.deltaTime * 7f);
            }
            else
            {
                //StopCoroutine(Following());
                Destroy(gameObject);
                //StageManager._SMInstance.NextStage();
                //break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private bool Targeting()
    {
        Vector2 myPos = transform.position;
        GameObject[] go = GameObject.FindGameObjectsWithTag("Touchable");
        float dis=0;
        int targetID=0;
        if (go.Length == 0) { _targetObject = null; return false; }
        for (int i = 0; i < go.Length; i++)
        {
            float dis2 = Vector2.Distance(myPos, go[i].transform.position);
            if (i == 0)
            { dis = dis2; targetID = 0; continue; }
            else if(dis2 < dis)
            {
                dis = dis2;
                targetID = i;
            }
        }
        _targetObject = go[targetID];
        return true;
    }

    private bool chainTargeting()
    {
        Vector2 myPos = transform.position;
        GameObject[] go = GameObject.FindGameObjectsWithTag("Touchable");
        float dis = -1;
        int targetID = -1;
        int chainedTargetID;
        if (go.Length == 0) { _targetObject = null; return false; }
        for (int i = 0; i < go.Length; i++)
        {
            if (go[i].GetComponent<EnemyCtrl>().IsChained()) { chainedTargetID = i; continue; }
            float dis2 = Vector2.Distance(myPos, go[i].transform.position);
            if (dis == -1)
            { dis = dis2; targetID = i; continue; }
            else if (dis2 < dis)
            {
                dis = dis2;
                targetID = i;
            }
        }
        if (targetID == -1) { return false; }
        _targetObject = go[targetID];
        return true;
    }

    private void SelectTargeting(int AbilityCode)
    {
        _targetObject = StageManager._SMInstance._abilityObj[AbilityCode - 1];
    }

    public void SetAttackEnergy(long bibleEnergy)
    {
        _attackEnergy = bibleEnergy;
    }

    public long GetAttackEnergy()
    {
        return _attackEnergy;
    }

    public void ShowDamageUIandSound()
    {
        AudioManager._am.StageAttackSoundPlay(0);
        Vector2 pos = _mainCamera.WorldToScreenPoint(transform.transform.position);
        (Instantiate(damageType[0], pos, Quaternion.identity, _damageUI.transform) as GameObject).GetComponent<DamageCtrl>().SetText(_attackEnergy);
    }

    public int GetStrongTarget()
    {
        return _strongTarget;
    }
}
