using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField]
    public GameObject explosion;
    public float explosionTime = 1f;

    public void Start()
    {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTime);
        GameObject explosionClone = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionClone, .5f);
        Destroy(gameObject);
    }
}
