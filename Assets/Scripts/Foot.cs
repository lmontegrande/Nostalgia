using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

    public int jumpDamage = 1;

    public delegate void onLandHandler();
    public delegate void onLeaveGroundHandle();
    public delegate void onJumpOnEnemyHandle(Enemy enemy);
    public onLandHandler onLand;
    public onJumpOnEnemyHandle onJumpOnEnemyHandler;
    public onLeaveGroundHandle onLeaveGround;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Floor" || other.tag == "Bomb" || other.tag == "Enemy" || other.tag == "Player")
        {
            onLand.Invoke();
        }
        if (other.tag == "Enemy")
        {
            onJumpOnEnemyHandler.Invoke(other.GetComponent<Enemy>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Floor" || other.tag == "Bomb" || other.tag == "Enemy" || other.tag == "Player")
        {
            onLeaveGround.Invoke();
        }
    }
}
