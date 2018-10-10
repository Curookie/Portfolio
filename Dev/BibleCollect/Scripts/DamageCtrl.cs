using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageCtrl : MonoBehaviour {

    private Text DamageText;

    private void Awake()
    {
        DamageText = transform.Find("Text").GetComponent<Text>();
    }

    public void SetText(long damage)
    {
        DamageText.text = NumberManager.NtoS(damage);
    }
}
