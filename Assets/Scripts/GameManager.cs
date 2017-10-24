using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public delegate void OnScoredHandler(int x);
    public OnScoredHandler OnScore; 

    public static GameManager instance;
    
    private int score; 

    public void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void AddScore(int val)
    {
        score += val;

        if (OnScore != null)
        {
            OnScore.Invoke(score);
        } 
    } 
}
