using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dog : MonoBehaviour {

    public int health = 5;
    public int jumpDamage = 1;
    public float jumpForce = 10f;
    public float dashForce = 5f;
    public float runSpeed = 5f;
    public int maxJumps = 2;
    public float dashDistance = 1f;
    public float dashTime = 1f;
    public float dashCooldown = 1f;
    public float bombSpawnOffset = 2f;
    public float bombAimedThrowStrength = 10f;
    public float bombNeutralThrowStrength = 1.5f;
    public float bombExplosionTime = 1f;
    public int bonusExplosions = 0;
    public int maxBombExplosions = 3;
    public float invincibiltyDamageTimer = .2f;
    public int playerNum = 1;
    public Foot foot;
    public GameObject doubleJumpParticle;
    public GameObject bomb;
    public WallCollisionDetector leftCollider;
    public WallCollisionDetector rightCollider;
    public Text playerNameText;
    public Text healthText;
    public Text coinText;
    public AudioClip jumpAudioClip;
    public AudioClip ouchAudioClip;
    public GameObject[] bonusBombSprites;
    public int lastAttackerPlayerNum;

    [HideInInspector]
    public bool isDead = false;

    private LineRenderer _lineRenderer;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private int numJumps;
    private int coinScore = 0;
    private bool onGround;
    private bool isDashing;
    private bool hasJumpDash;
    private bool isAiming = false;
    private float dashCooldownTimer = 100f;
    private float getHitCooldownTimer = 0f;
    private Color playerColor;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _lineRenderer = GetComponent<LineRenderer>();

        foot.onLand += OnLand;
        foot.onLeaveGround += OnLeaveGround;
        foot.onJumpOnEnemy += OnJumpOnEnemy;
        foot.onJumpOnOtherPlayer += OnJumpOnOtherPlayer;

        getHitCooldownTimer = invincibiltyDamageTimer;
        healthText.text = health.ToString();
        coinText.text = coinScore.ToString();
        playerNameText.text = "Player " + playerNum.ToString();
        switch(playerNum)
        {
            case 1:
                playerColor = GameManager.instance.player1Color;
                break;
            case 2:
                playerColor = GameManager.instance.player2Color;
                break;
            case 3:
                playerColor = GameManager.instance.player3Color;
                break;
            case 4:
                playerColor = GameManager.instance.player4Color;
                break;
            case 5:
                playerColor = GameManager.instance.player5Color;
                break;
        }

        playerNameText.color = playerColor;
        _lineRenderer.positionCount = 2;
        GameManager.instance.PlayerJoined();
        for(int x=0; x<bonusBombSprites.Length; x++)
        {
            bonusBombSprites[x].SetActive(false);
        }
    }

    public void Update()
    {
        if (isDashing || isDead) return;

        HandleAnimation();
        HandleInput();
        HandleCooldowns();
    }
  
    public void GetHit(int damage)
    { 
        if (getHitCooldownTimer <= invincibiltyDamageTimer)
            return;

        GetHitDirect(damage);
    }

    public void GetHit(int damage, int attackerPlayerNum)
    {
        if (getHitCooldownTimer <= invincibiltyDamageTimer)
            return;

        lastAttackerPlayerNum = attackerPlayerNum;
        GetHitDirect(damage);
    } 

    public void GetHitDirect(int damage)
    {
        getHitCooldownTimer = 0;
        _audioSource.PlayOneShot(ouchAudioClip);
        StartCoroutine(Blink(invincibiltyDamageTimer));
        health = (int) Mathf.Clamp(health - damage, 0, Mathf.Infinity); 
        healthText.text = health.ToString();
        _rigidBody.velocity = Vector2.up * jumpForce;
        if (health <= 0)
        {
            Die();
        }
    }

    public void GainCoin(int coinVal)
    {
        coinScore += coinVal;
        coinText.text = coinScore.ToString();

        bonusExplosions = Mathf.Clamp(coinScore / 2, 0, maxBombExplosions);
        for (int x=0; x<bonusExplosions; x++)
        {
            bonusBombSprites[x].SetActive(true);
        }
    }

    private void Die()
    {
        // TODO
        if (isDead) return;

        isDead = true;
        GameManager.instance.PlayerDied(this);
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(gameObject, 1f);
    }

    private void HandleInput()
    {
        // Attack
        HandleAttack();
        if (isAiming)
            return;

        // Movement
        HandleAxisInput();
        HandleJump();
        HandleDash();
    }

    private void HandleAxisInput()
    {
        bool touchingWallInWrongDirection = false;
        if (Input.GetAxisRaw("Horizontal_Player" + playerNum) >= 0.1 && rightCollider.isHittingWall) touchingWallInWrongDirection = true;
        if (Input.GetAxisRaw("Horizontal_Player" + playerNum) <= 0.1 && leftCollider.isHittingWall) touchingWallInWrongDirection = true;
        //if (Input.GetAxisRaw("Vertical_Player" + playerNum) <= -0.5 && !onGround) _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, -jumpForce);
        if (!touchingWallInWrongDirection)
            _rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal_Player" + playerNum) * runSpeed, _rigidBody.velocity.y);
    }

    private void HandleAttack()
    {
        isAiming = Input.GetButton("Fire3_Player" + playerNum);
        Vector3 axisInput = new Vector3(Input.GetAxisRaw("Horizontal_Player" + playerNum), Input.GetAxisRaw("Vertical_Player" + playerNum), 0);

        // Aiming Line
        if (isAiming)
            _lineRenderer.SetPositions(new Vector3[] { gameObject.transform.position, gameObject.transform.position + (axisInput * 1.5f) });
        else
            _lineRenderer.SetPositions(new Vector3[] { gameObject.transform.position, gameObject.transform.position});

        // Throw Bomba
        if (Input.GetButtonDown("Fire1_Player" + playerNum))
        {
            GameObject bombClone = Instantiate(bomb, transform.position + (axisInput.normalized * bombSpawnOffset), Quaternion.identity);
            bombClone.GetComponent<SpriteRenderer>().color = playerColor;
            bombClone.GetComponent<Rigidbody2D>().velocity = axisInput * bombAimedThrowStrength;
            bombClone.GetComponent<Bomb>().explosionTime = bombExplosionTime;
            bombClone.GetComponent<Bomb>().bonusExplosions = bonusExplosions;
            bombClone.GetComponent<Bomb>().ownerPlayerNum = playerNum;
        }
    }

    private void HandleDash()
    {
        if (Input.GetButtonDown("Fire2_Player" + playerNum))
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
    
    private void HandleJump()
    { 
        //if (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical")>=0.1)
        if (Input.GetButtonDown("Jump_Player" + playerNum))
        {

            if (numJumps >= maxJumps || (!onGround && _rigidBody.velocity.y > dashForce * 2))
            {
                return;
            }

            numJumps++;

            Destroy(Instantiate(doubleJumpParticle, foot.transform.position, Quaternion.identity), .25f);

            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce); 

            _audioSource.PlayOneShot(jumpAudioClip);
        }
    }

    private void HandleCooldowns()
    {
        getHitCooldownTimer += Time.deltaTime;
        dashCooldownTimer += Time.deltaTime;
    }

    private void HandleAnimation()
    {
        Vector3 inputAxis = new Vector3(Input.GetAxisRaw("Horizontal_Player" + playerNum), Input.GetAxisRaw("Vertical_Player" + playerNum));
        _animator.SetFloat("x-speed", Mathf.Abs(inputAxis.x));
        if (inputAxis.x > 0)
            _spriteRenderer.flipX = true;
        if (inputAxis.x < 0)
            _spriteRenderer.flipX = false;

    }

    private void OnJumpOnEnemy(Enemy enemy)
    {
        enemy.GetHit(jumpDamage);
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);

        _audioSource.PlayOneShot(ouchAudioClip);
    }

    private void OnJumpOnOtherPlayer(Dog player)
    {
        player.GetHit(jumpDamage, playerNum);
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);

        _audioSource.PlayOneShot(ouchAudioClip);
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
            GetComponent<SpriteRenderer>().color = playerColor;
        }
        GetComponent<SpriteRenderer>().color = playerColor;
    }
}
