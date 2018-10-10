using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour {

    private long _enemyFullHP;
    private long _enemyHP;
    private int _enemyType;
    private readonly long[] _valuePerEnemyType = new long[] { -200L, -500L, -300L, -2000L, -3000L, -6000L };

    private Text _enemyHPText;
    private Slider _enemySlider;

    private bool _isHuman;

    //PoisoningDamaged
    private int _poisoningAttackCount;
    private long _poisoningDamage;
    private long _newPoisoningDamage;

    //FiringDamaged
    private int _firingAttackCount;
    private bool _isFiring = false;
    private long _firingDamage;

    //ChainingCheck
    private bool _isChained = false;

    private Animator _Ani;

    private GameObject _damageUI;
    private Camera _mainCamera;

    private bool _isDie = false;

    public GameObject[] damageType;

    public void Awake()
    {
        _enemyType = int.Parse(name.Substring(9, 2));
        if (_enemyType <= 3) _isHuman = true;
        else _isHuman = false;

        _damageUI = GameObject.FindGameObjectWithTag("DamageUI");
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _Ani = transform.Find("Sprite").GetComponent<Animator>();
        _enemySlider = transform.Find("EnemyHPType" + string.Format("{0:00}", _enemyType) + "/Slider").GetComponent<Slider>();
        _enemyHPText = _enemySlider.transform.Find("Info/Text").GetComponent<Text>();   
    }

    private void Start()
    {
        InitEnemyHP();
    }

    public void InitEnemyHP()
    {
        //Debug.Log("EnemyHPType" + string.Format("{0:00}", _enemyType) + "/Slider");
        _enemyFullHP = ((StageManager._SMInstance.GetStage()-1) * -20) + _valuePerEnemyType[_enemyType-1];
        _enemySlider.maxValue = -_enemyFullHP;
        ChangeHp(_enemyFullHP);
    }

    public long EnemyHP()
    {
        return _enemyHP;
    }

    public long EnemyFullHP()
    {
        return _enemyFullHP;
    }

    public void EnemyDie()
    {
        CancelInvoke();
        _poisoningAttackCount = 0;
        _firingAttackCount = 0;
        StageManager._SMInstance.EnemyDie();
        _Ani.SetTrigger("Die");
    }

    private void ChangeHp(long NewHp)
    {
        _enemyHP = NewHp;
        _enemyHPText.text = NumberManager.NtoS(NewHp) + "";
        _enemySlider.value = -NewHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shootable")
        {
            AttackCtrl a = collision.GetComponent<AttackCtrl>();
            switch (a.GetAttackType())
            {
                case 1:
                    DamagedEnemy(a.GetAttackEnergy());
                    Destroy(collision.gameObject);
                    break;
                case 2:
                    DamagedEnemy(a.GetAttackEnergy());
                    _poisoningDamage = Convert.ToInt64(a.GetAttackEnergy() * 0.2);
                    if (IsInvoking("PoisoningDamaged"))
                    {
                        _poisoningDamage = Convert.ToInt64(((10 - _poisoningAttackCount) * _newPoisoningDamage) * 0.1) + _poisoningDamage;
                        _poisoningAttackCount = 0;
                        CancelInvoke("PoisoningDamaged");
                    }
                    InvokeRepeating("PoisoningDamaged", 0.5f, 0.5f);
                    Destroy(collision.gameObject);
                    break;
                case 4:
                    HumanAttributeAttack(a.GetAttackEnergy());
                    Destroy(collision.gameObject);
                    break;
                case 5:
                    GhostAttributeAttack(a.GetAttackEnergy());
                    Destroy(collision.gameObject);
                    break;
                case 7:
                    if(!_isFiring) { 
                        DamagedEnemy(a.GetAttackEnergy());
                        InvokeRepeating("FiringDamaged", 0.5f, 0.5f);
                        _firingDamage = Convert.ToInt64(a.GetAttackEnergy() * 0.15);
                        _isFiring = true;
                    }
                    break;
                case 8:
                    if(!_isChained)
                    {
                        DamagedEnemy(a.GetAttackEnergy());
                        if (a.GetChainCount()<= 4)
                            _isChained = true;
                        a.ChainArrived();
                    }
                    break;
                case 9:
                    DamagedEnemy(a.GetAttackEnergy());
                    Destroy(collision.gameObject);
                    break;
                default:
                    break;
            }
            //Debug.Log(collision.GetComponent<AttackCtrl>().GetAttackEnergy());
        }
    }

    public void ShowDamageUIandSound(long HitDamage)
    {
        AudioManager._am.StageAttackSoundPlay(0);
        Vector2 pos = _mainCamera.WorldToScreenPoint(transform.position);
        (Instantiate(damageType[0], pos, Quaternion.identity, _damageUI.transform) as GameObject).GetComponent<DamageCtrl>().SetText(HitDamage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        AttackCtrl a = collision.GetComponent<AttackCtrl>();
        if (collision.tag == "Shootable" && a.GetAttackType() == 8)
        {
            _isChained = false;
        }
    }
    
    public bool IsChained()
    {
        return _isChained;
    }

    public void AllAtack(long Damaged)
    {
        DamagedEnemy(Damaged);
        AudioManager._am.StageAttackSoundPlay(0);
    }

    public void FiringDamaged()
    {
        DamagedEnemy(_firingDamage);
        _firingAttackCount++;
        if (_firingAttackCount >= 10)
        {
            _isFiring = false;
            _firingAttackCount = 0;
            CancelInvoke("FiringDamaged");
        }
    }

    public void PoisoningDamaged()
    {
        if (_poisoningAttackCount == 0)
            _newPoisoningDamage = _poisoningDamage;
        DamagedEnemy(_newPoisoningDamage);
        _poisoningAttackCount++;
        if (_poisoningAttackCount == 10)
        {
            _poisoningAttackCount = 0;
            CancelInvoke("PoisoningDamaged");
        }
    }

    public void DamagedEnemy(long HitDamage)
    {
        if (HitDamage >= -_enemyHP)
        {
            ShowDamageUIandSound(HitDamage);
            ChangeHp(0);
            if(!_isDie)
            {
                _isDie = true;
                EnemyDie();
            }

            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            ShowDamageUIandSound(HitDamage);
            ChangeHp(_enemyHP + HitDamage);
        }
    }

    public void HumanAttributeAttack(long BE)
    {
        if (_isHuman) BE *= 5;
        DamagedEnemy(BE);
    }

    public void GhostAttributeAttack(long BE)
    {
        if (!_isHuman) BE *= 5;
        DamagedEnemy(BE);
    }
}
