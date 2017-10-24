using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionDetector : MonoBehaviour {

    public delegate void OnHitWallHandler();
    public OnHitWallHandler OnWallHit;

    public bool isHittingWall;
    public bool onlyDetectWalls = true;

    [HideInInspector]
    public GameObject collider;

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!onlyDetectWalls || other.tag == "Floor")
        {
            if (OnWallHit != null)
            {
                OnWallHit.Invoke();
            }
            isHittingWall = true;
        }
        collider = other.gameObject;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!onlyDetectWalls || other.tag == "Floor")
        {
            if (OnWallHit != null)
            {
                OnWallHit.Invoke();
            }
            isHittingWall = false;
        }
        collider = null;
    }
}
