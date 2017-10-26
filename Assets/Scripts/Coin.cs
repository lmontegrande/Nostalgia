using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public AudioClip coinClip;
    public int coinValue = 1;
    public bool isBouncingCoin = false;
    public float bouncingCoinLifeSpan = 2f;

    public void Start()
    {
        if (!isBouncingCoin) return;

        StartCoroutine(Blink()); 
        Destroy(gameObject, bouncingCoinLifeSpan);
    }


    public void OnTriggerEnter2D(Collider2D other)
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
        if (isBouncingCoin)
        {
            yield return new WaitForSeconds(bouncingCoinLifeSpan/2);
        }
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0x00);
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0xFF);
        } 
    }
}
