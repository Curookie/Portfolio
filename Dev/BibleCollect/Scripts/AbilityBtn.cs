using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBtn : MonoBehaviour {

    public BibleManager bm;

    private void Start()
    {
        bm = BibleManager._bm;
    }

    public void Clicked()
    {
        bm.MoveAbilityTab(int.Parse(gameObject.name.Substring(6)));
    }
}
