using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBtn : MonoBehaviour {

    public BibleManager bm;

    private void Start()
    {
        bm = BibleManager._bm;
    }

    public void Clicked()
    {
        bm.MoveChaptertoVerse(int.Parse(gameObject.name));
        GetComponent<Outline>().enabled = false;
    }
}
