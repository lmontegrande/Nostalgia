using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    public int storySceneIndex;
    public int arenaSceneIndex;
    public Text winnerText;

    public void Start()
    { 
        if (!PersistentData.instance.newData)
        {
            winnerText.text = "";
            return;
        }

        PersistentData.instance.newData = false;

        float highestScore = Mathf.NegativeInfinity;
        int totalWinners = 0;
        for (int x=0; x<5; x++)
        {
            if (PersistentData.instance.totalScore[x] > highestScore)
            {
                highestScore = PersistentData.instance.totalScore[x];
            }
        }

        string winnerstring = "Winner: \n";

        for (int x=0; x<5; x++)
        {
            if (PersistentData.instance.totalScore[x] == highestScore)
            {
                winnerstring += " Player " + (x+1 + " ").ToString();
                totalWinners++;
            }

            PersistentData.instance.totalScore[x] = 0;
        }

        if (totalWinners <= 0)
            winnerText.text = "";
        else
            winnerText.text = winnerstring;
    }

	public void LoadStory()
    {
        SceneManager.LoadScene(storySceneIndex);
    }
	
	public void LoadArena()
    {
        SceneManager.LoadScene(arenaSceneIndex);
    }
}
