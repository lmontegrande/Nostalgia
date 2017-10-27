using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int bombDamage = 1;
    public float bombForce = 10;
    public float bonusBombOffset = 1f;
    public float bonusBombDelay = .25f;
    public float bombLifeTime = .45f;

    [HideInInspector]
    public int ownerPlayerNum = -1;

    [HideInInspector]
    public int bonusExplosions;

    public void Start()
    { 
        StartCoroutine(BonusBombs());
        Destroy(gameObject, bombLifeTime);
    }

    public IEnumerator BonusBombs()
    {
        yield return new WaitForSeconds(bonusBombDelay);
        if (bonusExplosions > 0)
        {
            GameObject bombUp = Instantiate(gameObject, transform.position + (Vector3.up * bonusBombOffset), Quaternion.identity);
            GameObject bombDown = Instantiate(gameObject, transform.position + (Vector3.down * bonusBombOffset), Quaternion.identity);
            GameObject bombLeft = Instantiate(gameObject, transform.position + (Vector3.left * bonusBombOffset), Quaternion.identity);
            GameObject bombRight = Instantiate(gameObject, transform.position + (Vector3.right * bonusBombOffset), Quaternion.identity);

            bombUp.GetComponent<Explosion>().bonusExplosions = bonusExplosions - 1;
            bombDown.GetComponent<Explosion>().bonusExplosions = bonusExplosions - 1;
            bombLeft.GetComponent<Explosion>().bonusExplosions = bonusExplosions - 1;
            bombRight.GetComponent<Explosion>().bonusExplosions = bonusExplosions - 1;

            Destroy(bombDown.GetComponent<AudioSource>());
            Destroy(bombLeft.GetComponent<AudioSource>());
            Destroy(bombRight.GetComponent<AudioSource>());
        }
    }

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Floor")
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<Enemy>().GetHit(bombDamage);
            }
            if (other.tag == "Player")
            {
                other.GetComponent<Dog>().GetHit(bombDamage, ownerPlayerNum);
            }
            if (other.tag == "Floor")
            {
                
                Vector3 forceAmount = (other.transform.position - gameObject.transform.position).normalized * bombForce;
                other.GetComponent<RespawningFloor>().OnGetHit(forceAmount);
            }
        } 
    }
}
