﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int respawnsPermitted = 1;
    public bool versusMode = false;
    public float playerSpawnOffset = 1f;
    public GameObject dogPrefab;
    public AudioClip playerJoinedAudioClip;

    public Color player1Color, player2Color, player3Color, player4Color, player5Color;

    public delegate void OnScoredHandler(int x);
    public OnScoredHandler OnScore;

    public static GameManager instance;

    private int playersLeft;
    private int score;

    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    private GameObject player5;

    private int[] respawnsUsed = new int[5];

    public void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Start_Player1"))
        {
            if (player1 != null || (respawnsUsed[0] >= respawnsPermitted)) return;
            respawnsUsed[0]++;
            player1 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player1.GetComponent<Dog>().playerNum = 1;
            player1.GetComponent<SpriteRenderer>().color = player1Color;
        }
        if (Input.GetButtonDown("Start_Player2"))
        {
            if (player2 != null || (respawnsUsed[1] >= respawnsPermitted)) return;
            respawnsUsed[1]++;
            player2 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x + playerSpawnOffset, Camera.main.transform.position.y, 0), Quaternion.identity);
            player2.GetComponent<Dog>().playerNum = 2;
            player2.GetComponent<SpriteRenderer>().color = player2Color;
        }
        if (Input.GetButtonDown("Start_Player3"))
        {
            if (player3 != null || (respawnsUsed[2] >= respawnsPermitted)) return;
            respawnsUsed[2]++;
            player3 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x + playerSpawnOffset * 2, Camera.main.transform.position.y, 0), Quaternion.identity);
            player3.GetComponent<Dog>().playerNum = 3;
            player3.GetComponent<SpriteRenderer>().color = player3Color;
        }
        if (Input.GetButtonDown("Start_Player4"))
        {
            if (player4 != null || (respawnsUsed[3] >= respawnsPermitted)) return;
            respawnsUsed[3]++;
            player4 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x - playerSpawnOffset, Camera.main.transform.position.y, 0), Quaternion.identity);
            player4.GetComponent<Dog>().playerNum = 4;
            player4.GetComponent<SpriteRenderer>().color = player4Color;
        }
        if (Input.GetButtonDown("Start_Player5"))
        {
            if (player5 != null || (respawnsUsed[4] >= respawnsPermitted)) return;
            respawnsUsed[4]++;
            player5 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x - playerSpawnOffset * 2, Camera.main.transform.position.y, 0), Quaternion.identity);
            player5.GetComponent<Dog>().playerNum = 5;
            player5.GetComponent<SpriteRenderer>().color = player5Color;
        }
    }

    public void PlayerJoined()
    {
        GetComponent<AudioSource>().PlayOneShot(playerJoinedAudioClip);
        playersLeft++;
    }

    public void PlayerDied(Dog player)
    {
        playersLeft--;
        if (versusMode) {
            if (playersLeft <= 1)
                ArenaManager.instance.EndGame();

            ArenaManager.instance.PlayerDied(player);
        } else {
            if (playersLeft <= 0)
                StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);   
    }

    public void AddScore(int val)
    {
        score += val;

        if (OnScore != null)
        {
            OnScore.Invoke(score);
        } 
    } 

    public Color GetPlayerColor(int playerNum)
    {
        Color color = Color.black;
        switch(playerNum)
        {
            case 1:
                color = player1Color;
                break;
            case 2:
                color = player2Color;
                break;
            case 3:
                color = player3Color;
                break;
            case 4:
                color = player4Color;
                break;
            case 5:
                color = player5Color;
                break;
        }

        return color;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex);
    }
}
