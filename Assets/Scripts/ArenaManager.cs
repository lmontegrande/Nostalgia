using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaManager : MonoBehaviour {

    public int[] killScore;
    public int[] suicideScore;
    public float roundTimer = 30f;
    public float roundOverDelay = 0.5f;
    public Text timeText;
    public Text[] playerScoreText;
    public GameObject backgroundImage;
    public int nextSceneBuildIndex;

    public static ArenaManager instance;

    private float startingTime;

    private bool isGameDone = false;

    public void Start()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;

        killScore = new int[5];
        suicideScore = new int[5];
        startingTime = roundTimer;

        StartCoroutine(TimeGame());
    }

    public void Update()
    {
        if (isGameDone)
        {
            if (Input.GetButtonDown("Start_Player1") || Input.GetButtonDown("Start_Player2") || Input.GetButtonDown("Start_Player3") || Input.GetButtonDown("Start_Player4") || Input.GetButtonDown("Start_Player5"))
            {
                SceneManager.LoadScene(nextSceneBuildIndex);
            }
        }
    }

    public float GetTimeProportionLeft()
    {
        return roundTimer / startingTime;
    }

    public void PlayerDied(Dog playerWhoDied)
    {
        if (isGameDone) return;

        int attackerNum = playerWhoDied.lastAttackerPlayerNum;
        if (attackerNum >= 1 && attackerNum <= 5 && (attackerNum != playerWhoDied.playerNum))
            killScore[attackerNum-1]++;
        else
            suicideScore[playerWhoDied.playerNum-1]++;
    }

    public void EndGame()
    {
        StartCoroutine(EndGameDelayed());
    }

    private IEnumerator TimeGame()
    {
        bool isDone = false;
        while (!isDone)
        {
            yield return null;
            if (isGameDone)
                break;

            roundTimer -= Time.deltaTime;
            roundTimer = Mathf.Clamp(roundTimer, 0, Mathf.Infinity);
            timeText.text = ((int)roundTimer).ToString();
            if (roundTimer <= 0)
                isDone = true;
        }

        EndGame();
    }

    private IEnumerator EndGameDelayed()
    {
        if (!isGameDone)
        {
            yield return new WaitForSeconds(roundOverDelay);
            backgroundImage.SetActive(true);
            isGameDone = true;

            for (int x = 0; x < 5; x++)
            {
                PersistentData.instance.totalScore[x] = killScore[x] + -suicideScore[x] + PersistentData.instance.totalScore[x];
                string scoreText = "Round: +" + killScore[x] + ", -" + suicideScore[x] + " \nTotal: " + PersistentData.instance.totalScore[x];
                playerScoreText[x].text = scoreText;
                playerScoreText[x].color = GameManager.instance.GetPlayerColor(x+1);
                playerScoreText[x].gameObject.SetActive(true);

                timeText.gameObject.SetActive(false);
            }

            PersistentData.instance.newData = true;
        }
    }
}
