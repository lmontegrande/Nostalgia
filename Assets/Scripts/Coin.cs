using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public AudioClip coinClip;
    public int coinValue = 1;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().PlayOneShot(coinClip);
            GetComponent<Animator>().speed = 3;
            other.GetComponent<Dog>().GainCoin(coinValue);
            GameManager.instance.AddScore(coinValue);
            coinValue = 0;

            Destroy(GetComponent<CircleCollider2D>());
            StartCoroutine(Blink());
            Destroy(gameObject, .5f);
            Destroy(GetComponent<Rigidbody2D>());
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0x00);
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0xFF);
        }
        
    }
}
