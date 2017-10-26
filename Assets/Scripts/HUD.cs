﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    private Text scoreText;

    public void Start()
    {
        GameManager.instance.OnScore += (int x) => {
            scoreText.text = x.ToString();
        }; 
    } 
}
