using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public float moveFactor = .25f;
    private Camera camera;

    public void Start()
    {
        camera = Camera.main;
    }

    public void Update()
    {
        Vector3 target = camera.transform.position - camera.transform.position * moveFactor;
        transform.position = new Vector3(target.x, target.y, transform.position.z);
    }
}
