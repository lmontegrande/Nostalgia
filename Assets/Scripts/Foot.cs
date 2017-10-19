using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

    public delegate void onLandHandler();
    public delegate void onLeaveGroundHandle();
    public onLandHandler onLand;
    public onLeaveGroundHandle onLeaveGround;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Floor")
        {
            onLand.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Floor")
        {
            onLeaveGround.Invoke();
        }
    }
}
