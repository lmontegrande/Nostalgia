using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    private Text text;

    public void Start()
    {
        GameManager.instance.OnScore += ScoreChange;
    }	

    private void ScoreChange (int x)
    {
        text.text = x.ToString();
    }
}
