using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

    public int jumpDamage = 1;

    public delegate void onLandHandler();
    public delegate void onLeaveGroundHandle();
    public delegate void onJumpOnEnemyHandle(Enemy enemy);
    public delegate void onJumpOnOtherPlayerHandle(Dog dog);
    public onLandHandler onLand;
    public onJumpOnEnemyHandle onJumpOnEnemy;
    public onLeaveGroundHandle onLeaveGround;
    public onJumpOnOtherPlayerHandle onJumpOnOtherPlayer;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Floor" || other.tag == "Bomb" || other.tag == "Enemy" || other.tag == "Player")
        {
            onLand.Invoke();
        }
        if (other.tag == "Enemy")
        {
            onJumpOnEnemy.Invoke(other.GetComponent<Enemy>());
        }
        if (other.tag == "Player")
        {
            onJumpOnOtherPlayer.Invoke(other.GetComponent<Dog>());
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
