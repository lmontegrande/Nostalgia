using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningFloor : MonoBehaviour {

    public bool isRespawningFloor = false;
    public float respawnTime = 1f;
    public float invulnerableTime = 1f;

    [HideInInspector]
    public bool isCurrentlyRespawning;

    private Color startingColor;
    private Vector3 startingPosition;

    public void Start()
    {
        startingPosition = transform.position;
        startingColor = GetComponent<SpriteRenderer>().color;
    }

	public void OnGetHit(Vector3 flyVelocity)
    {
        GetComponent<Rigidbody2D>().velocity = flyVelocity;
        GetComponent<SpriteRenderer>().color = Color.black;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        if (isRespawningFloor)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            transform.position = startingPosition;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Coroutine c = StartCoroutine(Blink());
            yield return new WaitForSeconds(invulnerableTime);
            GetComponent<BoxCollider2D>().enabled = true;
            StopCoroutine(c);
            GetComponent<SpriteRenderer>().color = startingColor;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        } else {
            Destroy(gameObject);
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
