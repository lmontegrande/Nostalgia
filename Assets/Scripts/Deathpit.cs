using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathpit : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Dog>().GetHit(1000);
        } else if (other.tag == "Enemy") {
            other.GetComponent<Enemy>().GetHit(1000);
        }
    }
}
