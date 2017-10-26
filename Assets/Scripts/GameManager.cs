using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool versusMode = false;
    public GameObject dogPrefab;

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
            if (player1 != null) return;
            player1 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player1.GetComponent<Dog>().playerNum = 1;
            player1.GetComponent<SpriteRenderer>().color = player1Color;
        }
        if (Input.GetButtonDown("Start_Player2"))
        {
            if (player2 != null) return;
            player2 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player2.GetComponent<Dog>().playerNum = 2;
            player2.GetComponent<SpriteRenderer>().color = player2Color;
        }
        if (Input.GetButtonDown("Start_Player3"))
        {
            if (player3 != null) return;
            player3 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player3.GetComponent<Dog>().playerNum = 3;
            player3.GetComponent<SpriteRenderer>().color = player3Color;
        }
        if (Input.GetButtonDown("Start_Player4"))
        {
            if (player4 != null) return;
            player4 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player4.GetComponent<Dog>().playerNum = 4;
            player4.GetComponent<SpriteRenderer>().color = player4Color;
        }
        if (Input.GetButtonDown("Start_Player5"))
        {
            if (player5 != null) return;
            player5 = Instantiate(dogPrefab, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0), Quaternion.identity);
            player5.GetComponent<Dog>().playerNum = 5;
            player5.GetComponent<SpriteRenderer>().color = player5Color;
        }
    }

    public void PlayerJoined()
    {
        playersLeft++;
    }

    public void PlayerDied()
    {
        playersLeft--;
        if (versusMode) { 
            if (playersLeft <= 1)
                StartCoroutine(EndGame());
        } else {
            if (playersLeft <= 0)
                StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
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
