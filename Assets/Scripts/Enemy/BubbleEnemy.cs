using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEnemy : Enemy {

    public int attackDamage = 1;
    public float moveSpeed = 1f;
    public bool moveLeftFirst = true;
    public float deathFlyForce = 20f;

    [SerializeField]
    private WallCollisionDetector leftWallCD, rightWallCD;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private float currentMoveDirection;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        currentMoveDirection = moveLeftFirst ? -Mathf.Abs(moveSpeed) : Mathf.Abs(moveSpeed);

        leftWallCD.OnWallHit = () => 
        {
            currentMoveDirection = Mathf.Abs(moveSpeed);
            _spriteRenderer.flipX = true;
        };

        rightWallCD.OnWallHit = () =>
        {
            currentMoveDirection = -Mathf.Abs(moveSpeed);
            _spriteRenderer.flipX = false;
        };

        StartCoroutine(Move());
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Dog>().GetHit(attackDamage);
        }
    }

    protected override void Die()
    {
        isDead = true;
        Destroy(GetComponent<CircleCollider2D>());
        //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, deathFlyForce);
        Destroy(gameObject, 1f);
        StartCoroutine(Blink());
    }

    private IEnumerator Move()
    {
        if (_rigidbody.velocity.x > 0)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;

        while (!isDead) {
            _rigidbody.velocity = new Vector2(currentMoveDirection, _rigidbody.velocity.y);
            _animator.SetFloat("x-speed", Mathf.Abs(currentMoveDirection));
            yield return null;
        }
    }
}
