using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;

	public void Update()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
