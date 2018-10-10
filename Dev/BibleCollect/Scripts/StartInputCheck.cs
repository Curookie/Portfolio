using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartInputCheck : MonoBehaviour {

    public Text txtName;
    public Toggle ckMan;
    public Toggle ckWoman;
	
    public void Checking()
    {
        if(txtName.text.Length==0)
        {
            return;
        }
        if (ckMan.isOn || ckWoman.isOn)
        {
            GameObject.Find("Input").SetActive(false);
            return;
        }
        return;
    }
}
