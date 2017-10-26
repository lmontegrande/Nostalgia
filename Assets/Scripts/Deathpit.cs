using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathpit : MonoBehaviour {

    public bool deathBoundary = false;

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Dog>().GetHitDirect(1000);
        } else if (other.tag == "Enemy" && !deathBoundary) {
            other.GetComponent<Enemy>().GetHitDirect(1000);
        }
    }
}
