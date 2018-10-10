using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCtrl : MonoBehaviour {

    public GameObject _attackSlider;
    public GameObject _attackBall;
    public GameObject _attack2thBall;


    private readonly float[] _attackDelayList = new float[] { 1.0f, 1.5f, 1.0f, 1.0f, 1.0f, 3.0f, 3.0f, 1.5f, 3.0f };
    private float _attackDelay;

    private long _bibleEnergy = 0L;

    private bool _isDragging = false;
    private bool _isAbilityImageOn = false;
    private bool _isAttacking = false;

    private int _abilityCode;
    private int _abilityAttackCountMax;
    private int _abilityAttackCount = 5;

    private Vector2 _startPos;

    private Camera _gameCamera;
    private SpringJoint2D _spring;
    private CanvasGroup _attackType;
    private Text _beText;
    private Text _acText;
    private AttackCtrl _attackCtrl;
    private Image _attackCountImage;

    private float _timer = 0.0f;
    private float _yMaxOffset = 13.0f;
    private float _yMinOffset = -11.0f;
    private float _xMinOffset = -8.0f;
    private float _xMaxOffset = 8.0f;

    private float _invokeTriCnt = 0;
    private float _invokeThiCnt = 0;

    private void Awake()
    {
        _abilityCode = int.Parse(name.Substring(7));
        _spring = GetComponent<SpringJoint2D>();
        _spring.connectedAnchor = transform.position;
        _startPos = transform.position;
    }

    private void Start()
    {
        if (DataManager._dm != null)
            _bibleEnergy = System.Convert.ToInt64(10 + DataManager._dm.GetBibleEnergyPerAbility(_abilityCode) * 0.2);
        else
            _bibleEnergy = 10L;
        _beText = transform.Find("AbilityUI/Info/Text").GetComponent<Text>();
        _beText.text = "+" + NumberManager.NtoS(_bibleEnergy) + "";
        _gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _attackDelay = _attackDelayList[_abilityCode - 1];
        _attackSlider.GetComponent<Slider>().maxValue = _attackDelay;
        _attackType = transform.Find("AbilityUI/AttackType").GetComponent<CanvasGroup>();
        _abilityAttackCountMax = _abilityAttackCount;
        _acText = transform.Find("AbilityUI/AttackCount/Text").GetComponent<Text>();
        _acText.text = _abilityAttackCount + "";
        _attackCountImage = _acText.transform.parent.GetComponent<Image>();
    }

    private void OnMouseDown()
    {
        _spring.enabled = true;
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        //spring.enabled = false;
        _isDragging = false;
        _attackType.alpha = 0;
        _isAbilityImageOn = false;
        if (_abilityAttackCount == 0 )
        {
        } else if (_spring.connectedAnchor.y >= -5.5 && _isAttacking == false) {
            StartAttack();
        } else
        {
            StopAttack();
        }
    }
    
    private void StartAttack()
    {

        _attackCountImage.color = new Color32(0xF2, 0x49, 0x49, 0xFF);
        _attackSlider.SetActive(true);
        _isAttacking = true;
        StartCoroutine(CreateAttack());
        _timer = 0.0f;
        _attackSlider.GetComponent<Slider>().value = _timer;
    }

    private void StopAttack()
    {
        if (_abilityCode == 6 || _abilityCode == 7) transform.Find("Ani").GetComponent<Animator>().SetTrigger("Cancel");
        _attackCountImage.color = new Color32(0x4B, 0x4B, 0x4B, 0xFF);
        _attackSlider.SetActive(false);
        _isAttacking = false;
        StopCoroutine(CreateAttack());
        _timer = 0.0f;
        _attackSlider.GetComponent<Slider>().value = _timer;
    }


    private IEnumerator CreateAttack()
    {
        while (_isAttacking)
        {
            _timer = 0.0f;
            _attackSlider.GetComponent<Slider>().value = _timer;
            if (_abilityCode == 6) transform.Find("Ani").GetComponent<Animator>().SetTrigger("Ready");
            if (_abilityCode == 7) transform.Find("Ani").GetComponent<Animator>().SetTrigger("Ready7");
            yield return new WaitForSeconds(_attackDelay);
            //Debug.Break();
            if (_abilityAttackCount == 0 || _isAttacking == false || StageManager._SMInstance.GetEnemyCount() == 0)
            {  StopAttack(); break; }
            if (_timer < _attackDelay)
                break;
            Attacking();
            if (_abilityAttackCount == 0)
            {
                StopAttack(); break;
            }
        }
    }

    private void Update()
    {
        if (_isAttacking && _timer!=_attackDelay)
        {
            _timer += Time.deltaTime;
            _attackSlider.GetComponent<Slider>().value = _timer;
        }
    }

    private void Attacking()
    {
        if (_abilityCode == 2)
        {
            InvokeRepeating("TripleAttack", 0f, 0.25f);
        } else if (_abilityCode == 3)
        {
            _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
            _attackCtrl.SetAttackEnergy(_bibleEnergy);
            _attackCtrl = (Instantiate(_attack2thBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
            _attackCtrl.SetAttackEnergy(_bibleEnergy);
        } else if (_abilityCode == 6)
        {
            GameObject[] all = GameObject.FindGameObjectsWithTag("Touchable");
            foreach(GameObject enemy in all)
            {
                enemy.GetComponent<EnemyCtrl>().AllAtack(_bibleEnergy);
            }
        } else if (_abilityCode == 7) {
            _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity) as GameObject).GetComponent<AttackCtrl>();
            _attackCtrl.SetAttackEnergy(_bibleEnergy);
        } else if (_abilityCode == 9)
        {
            int i = Random.Range(0, 100);
            if(i<=49)
            {
                InvokeRepeating("ThirtyAttack", 0f, 0.2f);
            } else
            {
                _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
                _attackCtrl.SetAttackEnergy(_bibleEnergy);
            }
        } else 
        {
            _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
            _attackCtrl.SetAttackEnergy(_bibleEnergy);
        } 
        _abilityAttackCount -= 1;
        _acText.text = _abilityAttackCount + "";
        if (_abilityAttackCount == 0) StageManager._SMInstance.AbilityEnd();
    }

    private void TripleAttack()
    {
        _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
        _attackCtrl.SetAttackEnergy(_bibleEnergy);
        _invokeTriCnt++;
        if (_invokeTriCnt == 3)
        {
            _invokeTriCnt = 0;
            CancelInvoke("TripleAttack");
        }
    }

    private void ThirtyAttack()
    {
        _attackCtrl = (Instantiate(_attackBall, transform.position, Quaternion.identity, transform.parent) as GameObject).GetComponent<AttackCtrl>();
        _attackCtrl.SetAttackEnergy(_bibleEnergy * 3);
        _invokeThiCnt++;
        if (_invokeThiCnt == 10)
        {
            _invokeThiCnt = 0;
            CancelInvoke("ThirtyAttack");
        }
    }

    private void OnMouseDrag()
    {
        if(_spring.enabled == true)
        {
            Vector2 cursorPosition = _gameCamera.ScreenToWorldPoint(Input.mousePosition);
            if (cursorPosition.y > _yMaxOffset)
                cursorPosition.y = _yMaxOffset;
            if (cursorPosition.y < _yMinOffset)
                cursorPosition.y = _yMinOffset;
            if (cursorPosition.x > _xMaxOffset)
                cursorPosition.x = _xMaxOffset;
            if (cursorPosition.x < _xMinOffset)
                cursorPosition.x = _xMinOffset;
            _spring.connectedAnchor = cursorPosition;
        }
        if(!_isAbilityImageOn)
        {
            _attackType.alpha = 1;
            _isAbilityImageOn = true;
            StopAttack();
        }
    }

    public void StageReseted()
    {
        StopAttack();
        if (DataManager._dm != null)
            _bibleEnergy = System.Convert.ToInt64(10 + DataManager._dm.GetBibleEnergyPerAbility(_abilityCode) * 0.2);
        else
            _bibleEnergy = 10L;
        _beText.text = "+" + NumberManager.NtoS(_bibleEnergy) + "";
        _abilityAttackCount = _abilityAttackCountMax;
        _acText.text = _abilityAttackCount + "";
        _spring.connectedAnchor = _startPos;
    }

    private void StrongBE(long v)
    {
        _bibleEnergy += v;
        _beText.text = "+" + NumberManager.NtoS(_bibleEnergy) + "";
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        AttackCtrl a = collision.GetComponent<AttackCtrl>();

        if (collision.tag == "Strongable" && _abilityCode == a.GetStrongTarget())
        {
            StrongBE(a.GetAttackEnergy());
            Destroy(collision.gameObject);
            AudioManager._am.StageAttackSoundPlay(0);
        }
    }
}
