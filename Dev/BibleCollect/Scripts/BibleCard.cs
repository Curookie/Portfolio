
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BibleCard {

    [SerializeField]
    private int verseCode;
    [SerializeField]
    private int chapterCode;
    [SerializeField]
    private int testamentCode;
    [SerializeField]
    private int abilityCode;
    [SerializeField]
    private long bibleEnergy;
    [SerializeField]
    private int cardRareRate;

    public BibleCard(int vC, int cC, int tC,int aC, long bE, int cRR)
    {
        verseCode = vC;
        chapterCode = cC;
        testamentCode = tC;
        abilityCode = aC;
        bibleEnergy = bE;
        cardRareRate = cRR;
    }

    public int GetVerseCode()
    {
        return verseCode;
    }
    public int GetChapterCode()
    {
        return chapterCode;
    }
    public int GetTestamentCode()
    {
        return testamentCode;
    }
    public int GetCardRareRate()
    {
        return cardRareRate;
    }
    public long GetBibleEnergy()
    {
        return bibleEnergy;
    }
    public int GetAbilityCode()
    {
        return abilityCode;
    }
}
