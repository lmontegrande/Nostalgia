using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    public float jumpForce = 10f;
    public float dashForce = 5f;
    public float runSpeed = 5f;
    public int maxJumps = 2;
    public Foot foot;
    public GameObject doubleJumpParticle;
    public GameObject dashParticle;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private int numJumps;
    private bool onGround;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        foot.onLand += OnLand;
        foot.onLeaveGround += OnLeaveGround;
    }

    public void Update()
    {
        HandleAnimation();
        HandleInput();
    }

    private void HandleInput()
    {
        // Horizontal Movement
        _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, _rigidBody.velocity.y);

        // Vertical Movement
        HandleJump();
        HandleDash();
    }

    private void HandleDash()
    {
        //Needs Fixing
        return;

        if (Input.GetButtonDown("Fire2"))
        {
            if (_rigidBody.velocity.x >= 0.1)
            {
                Destroy(Instantiate(dashParticle, transform.position, Quaternion.identity), 0.1f);
                _rigidBody.velocity = (Vector2.right * dashForce * (1 / Time.deltaTime));
            }
            if (_rigidBody.velocity.x <= -0.1)
            {
                GameObject dash = Instantiate(dashParticle, transform.position, Quaternion.identity);
                dash.GetComponent<SpriteRenderer>().flipX = true;
                Destroy(dash, 0.1f);
                _rigidBody.velocity = (-Vector2.right * dashForce * (1 / Time.deltaTime));
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {

            if (numJumps >= maxJumps || (!onGround && _rigidBody.velocity.y > 0))
            {
                return;
            }

            numJumps++;

            Destroy(Instantiate(doubleJumpParticle, foot.transform.position, Quaternion.identity), .25f);

            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
            _rigidBody.AddForce(Vector2.up * jumpForce * (1 / Time.deltaTime));
        }
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("x-speed", Mathf.Abs(_rigidBody.velocity.x));
        if (_rigidBody.velocity.x > 0.1)
            _spriteRenderer.flipX = true;
        if (_rigidBody.velocity.x < 0.1)
            _spriteRenderer.flipX = false;

    }

    private void OnLand()
    {
        numJumps = 0;
        onGround = true;
    }

    private void OnLeaveGround()
    {
        onGround = false;
    }
}
