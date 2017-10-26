using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float lerpFactor = .5f;

    private GameObject[] players;
    private Vector3 startingPosition;
    private Camera _camera;

    public void Start()
    {
        startingPosition = transform.position; 
    }

	public void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        _camera = GetComponent<Camera>();

        Vector3 targetPosition = new Vector3();
        int aliveSize = 0;

        foreach(GameObject player in players)
        {
            if (!player.GetComponent<Dog>().isDead)
            {
                targetPosition += player.transform.position;
                aliveSize++;
            }
        }

        if (aliveSize <= 0) return;

        if (players.Length == 2) {
            Vector3 deltaVector = players[0].transform.position - players[1].transform.position;
            _camera.orthographicSize = (Mathf.Clamp(deltaVector.magnitude/2, 5, 10));
        }        

        targetPosition = Vector3.Lerp(gameObject.transform.position, targetPosition / aliveSize, lerpFactor);
        gameObject.transform.position = new Vector3(targetPosition.x, Mathf.Clamp(targetPosition.y, startingPosition.y, 1000), gameObject.transform.position.z);
    }
}
