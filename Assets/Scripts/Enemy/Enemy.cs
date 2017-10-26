using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int health = 1;
    public int attackStrength = 1;
    protected bool isDead = false;

    public virtual void GetHit(int damage)
    {
        // Invincibility Timer accounted for here
        GetHitDirect(damage);
    }

    public virtual void GetHitDirect(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject, 1f);
        StartCoroutine(Blink());
    }

    protected IEnumerator Blink()
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
