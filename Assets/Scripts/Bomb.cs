﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField]
    public GameObject explosion;

    [HideInInspector]
    public float explosionTime = 1f;

    [HideInInspector]
    public int bonusExplosions = 0;

    public void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTime);
        GameObject explosionClone = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionClone.GetComponent<Explosion>().bonusExplosions = bonusExplosions; 
        Destroy(gameObject);
    }
}
