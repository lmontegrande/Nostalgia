using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float lerpFactor = .5f;
    public float minOrthographicSize = 5f;
    public float maxOrthographicSize = 10f;

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

        targetPosition = Vector3.Lerp(gameObject.transform.position, targetPosition / aliveSize, lerpFactor);

        if (players.Length >= 2)
        {
            Vector3 deltaVector = Vector3.zero;
            foreach (GameObject player in players)
            {
                if ((targetPosition - player.transform.position).magnitude >= deltaVector.magnitude)
                {
                    deltaVector = player.transform.position - targetPosition; 
                    //deltaVector = player.transform.position - transform.position;
                }
            } 
 
            // Fix Camera Scaling for multiple characters
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, (Mathf.Clamp(deltaVector.magnitude / 2, minOrthographicSize, maxOrthographicSize)), Time.deltaTime);
        } else {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, minOrthographicSize, Time.deltaTime);
        }
        gameObject.transform.position = new Vector3(targetPosition.x, Mathf.Clamp(targetPosition.y, startingPosition.y, 1000), gameObject.transform.position.z);
    }
}
