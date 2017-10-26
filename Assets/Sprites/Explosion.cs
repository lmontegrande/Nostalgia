using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int bombDamage = 1;
    public float bombForce = 10;

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
                other.GetComponent<Dog>().GetHit(bombDamage);
            }
            if (other.tag == "Floor")
            {
                
                Vector3 forceAmount = (other.transform.position - gameObject.transform.position).normalized * bombForce;
                other.GetComponent<RespawningFloor>().OnGetHit(forceAmount);
            }
        }
    }
}
