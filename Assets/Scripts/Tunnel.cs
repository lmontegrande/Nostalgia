using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tunnel : MonoBehaviour {

    public int levelToLoad;
    public AudioClip tunnelAudioClip;
    public float loadDelay = 2f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        GetComponent<AudioSource>().PlayOneShot(tunnelAudioClip);
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(levelToLoad);
    }
}
