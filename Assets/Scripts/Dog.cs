using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    public int health = 5;
    public float jumpForce = 10f;
    public float dashForce = 5f;
    public float runSpeed = 5f;
    public int maxJumps = 2;
    public float dashDistance = 1f;
    public float dashTime = 1f;
    public float dashCooldown = 1f;
    public float bombSpawnOffset = 2f;
    public float invincibiltyDamageTimer = .2f;
    public Foot foot;
    public GameObject doubleJumpParticle;
    public GameObject bomb;
    public WallCollisionDetector leftCollider;
    public WallCollisionDetector rightCollider;
    public AudioClip jumpAudioClip;
    public AudioClip ouchAudioClip;

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private int numJumps;
    private bool onGround;
    private bool isDashing;
    private bool hasJumpDash;
    private float dashCooldownTimer = 100f;
    private float getHitCooldownTimer = 0f;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        foot.onLand += OnLand;
        foot.onLeaveGround += OnLeaveGround;

        getHitCooldownTimer = invincibiltyDamageTimer;
    }

    public void Update()
    {
        if (isDashing) return;

        HandleAnimation();
        HandleInput();
        HandleCooldowns();
    }

    public void GetHit(int damage)
    {
        // TODO
        if (getHitCooldownTimer <= invincibiltyDamageTimer)
            return;

        getHitCooldownTimer = 0;
        _audioSource.PlayOneShot(ouchAudioClip);
        StartCoroutine(Blink(invincibiltyDamageTimer));
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO
    }

    private void HandleInput()
    {
        // Attack
        HandleAttack(); 

        // Horizontal Movement
        bool touchingWallInWrongDirection = false;
        if (Input.GetAxisRaw("Horizontal") >= 0.1 && rightCollider.isHittingWall) touchingWallInWrongDirection = true;
        if (Input.GetAxisRaw("Horizontal") <= 0.1 && leftCollider.isHittingWall) touchingWallInWrongDirection = true;
        if (!touchingWallInWrongDirection)
            _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, _rigidBody.velocity.y);

        // Vertical Movement
        HandleJump();
        HandleDash();
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bombClone = Instantiate(bomb, transform.position + ((new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, 0)).normalized * bombSpawnOffset), Quaternion.identity);
            bombClone.GetComponent<Rigidbody2D>().velocity = _rigidBody.velocity;
        }
    }

    private void HandleDash()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!hasJumpDash && !onGround) return;

            if (dashCooldownTimer <= (dashCooldown + dashTime)) return;

            if (_rigidBody.velocity.x >= 0.1)
            {
                StartCoroutine(DashTo(gameObject.transform.position + (Vector3.right * dashDistance)));
            }
            if (_rigidBody.velocity.x <= -0.1)
            {
                StartCoroutine(DashTo(gameObject.transform.position + (-Vector3.right * dashDistance)));
            }
        }
    }

    private void HandleCooldowns()
    {
        getHitCooldownTimer += Time.deltaTime;
        dashCooldownTimer += Time.deltaTime;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") || (Input.GetButtonDown("Vertical")&&Input.GetAxisRaw("Vertical")>=0.1))
        {

            if (numJumps >= maxJumps || (!onGround && _rigidBody.velocity.y > dashForce * 2))
            {
                return;
            }

            numJumps++;

            Destroy(Instantiate(doubleJumpParticle, foot.transform.position, Quaternion.identity), .25f);

            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
            // This way is broken on inconsistent frame rate comps
            //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
            //_rigidBody.AddForce(Vector2.up * jumpForce * (1 / Time.deltaTime));

            _audioSource.PlayOneShot(jumpAudioClip);
        }
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("x-speed", Mathf.Abs(_rigidBody.velocity.x));
        if (_rigidBody.velocity.x > 0)
            _spriteRenderer.flipX = true;
        if (_rigidBody.velocity.x < 0)
            _spriteRenderer.flipX = false;

    }

    private void OnLand()
    {
        numJumps = 0;
        onGround = true;
        hasJumpDash = true;
    }

    private void OnLeaveGround()
    {
        onGround = false;
    }

    private IEnumerator DashTo(Vector2 targetLocation)
    {
        Vector2 startingLocation = gameObject.transform.position;
        float timer = 0f;
        isDashing = true;
        dashCooldownTimer = 0f;
        _animator.speed = 4;
        _animator.SetBool("dashing", true);

            if (!onGround)
        {
            hasJumpDash = false;
        }

        while (timer < dashTime) {

            // Collision Detection still clips.  Need to opt in to raycasting
            if (leftCollider.isHittingWall || rightCollider.isHittingWall)
            {
                break;
            }

            timer += Time.deltaTime;

            _rigidBody.MovePosition(Vector2.Lerp(startingLocation, targetLocation, timer / dashTime));

            yield return null;
        }

        _animator.speed = 1;
        _animator.SetBool("dashing", false);
        isDashing = false;
    }

    private IEnumerator Blink(float duration)
    {
        while (getHitCooldownTimer < duration)
        {
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0x00);
            yield return new WaitForSeconds(.05f);
            GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0xFF);
        }
        GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF, 0xFF);
    }
}
