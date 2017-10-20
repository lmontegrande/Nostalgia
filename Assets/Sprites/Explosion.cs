using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int bombDamage = 1;
    public float bombForce = 10;

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            Vector3 forceAmount = (other.transform.position - gameObject.transform.position).normalized * bombForce * (1 / Time.deltaTime);
            other.GetComponent<Rigidbody2D>().AddForce(forceAmount);
        }
    }
}
