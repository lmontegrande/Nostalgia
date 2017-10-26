using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour {

    public float moveSpeed = .1f;

    [SerializeField]
    public WallCollisionDetector leftWallCollisionDetector;

    [SerializeField]
    public WallCollisionDetector rightWallCollisionDetector;

    private Vector2 moveDirection = Vector2.right;
    
    Rigidbody2D _rigidBody;

    public void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (rightWallCollisionDetector.isHittingWall) moveDirection = -Vector2.right;
        if (leftWallCollisionDetector.isHittingWall) moveDirection = Vector2.right;

        _rigidBody.velocity = new Vector2(moveDirection.x * moveSpeed, _rigidBody.velocity.y);
    }
}
