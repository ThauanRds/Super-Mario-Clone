using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rbPlayer;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float currentSpeed;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] bool isJump;
    [SerializeField] bool inFloor = true;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    Animator animPlayer;

    [SerializeField] bool dead = false;
    CapsuleCollider2D playerCollider;

    public static bool isGrow;
    public static bool powerUp;
    public bool isFlower;
    [SerializeField] private bool isCrouch;
    public float xMove;

    public float starTimer = 10f;
    public float currentStarTime = 0f;


    private void Awake()
    {
        animPlayer = GetComponent<Animator>();
        rbPlayer = GetComponent<Rigidbody2D>();

        playerCollider = GetComponent<CapsuleCollider2D>();

        currentSpeed = moveSpeed;
    }

    private void Start()
    {
        dead = false;

        isGrow = false;
        powerUp = false;
        isFlower = false;
    }

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] private float force = 5f;
    private float nextFire;
    [SerializeField] float fireRate = 0.5f;

    private void Update()
    {
        if (dead) return;

        inFloor = Physics2D.Linecast(transform.position, groundCheck.position, groundLayer);
        Debug.DrawLine(transform.position, groundCheck.position, Color.red);

        animPlayer.SetBool("Jump", !inFloor);
        animPlayer.SetBool("Grow", isGrow);
        animPlayer.SetBool("PowerUp", powerUp);
        animPlayer.SetBool("isFlower", isFlower);

        if (Input.GetButtonDown("Jump") && inFloor)
            isJump = true;
        //pulo vari�vel
        else if (Input.GetButtonUp("Jump") && rbPlayer.velocity.y > 0)
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.y * 0.5f);

        if (isFlower)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > nextFire && !isCrouch)
            {
                animPlayer.SetTrigger("Shooting");
                GameObject tempBall = Instantiate(ballPrefab, shootPoint.position, shootPoint.rotation);
                if (transform.eulerAngles == Vector3.zero)
                    tempBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force, ForceMode2D.Impulse);
                else if(transform.eulerAngles == Vector3.up * 180f)
                    tempBall.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force, ForceMode2D.Impulse);

                nextFire = Time.time + fireRate;
            }
        }

        #region CROUCH PLAYER

        animPlayer.SetBool("Crouch", isCrouch);

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) && isGrow && inFloor)
        {
            moveSpeed -= 0.05f;
            if (moveSpeed <= 0)
                moveSpeed = 0;
            isCrouch = true;
        }

        else if(!Input.GetKey(KeyCode.DownArrow) || !Input.GetKey(KeyCode.S) && isGrow && inFloor)
        {
            moveSpeed = currentSpeed;
            isCrouch = false;
        }

        #endregion

        #region TEMPORIZADOR DA ESTRELA

        if (powerUp)
        {
            currentStarTime += Time.deltaTime;

            if (currentStarTime >= starTimer)
            {
                currentStarTime = 0;
                powerUp = false;
            }
        }

        #endregion
    }

    private void FixedUpdate()
    {
        Move();
        JumpPlayer();
    }

    void Move()
    {
        if (dead) return;

        if(!isCrouch)
        xMove = Input.GetAxis("Horizontal");

        rbPlayer.velocity = new Vector2(xMove * moveSpeed, rbPlayer.velocity.y);

        animPlayer.SetFloat("Speed", Mathf.Abs(xMove));

        if (xMove > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (xMove < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    void JumpPlayer()
    {
        if (dead) return;

        if (isJump)
        {
            rbPlayer.velocity = Vector2.up * jumpForce;
            isJump = false;
        }
    }

    public void Death()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        if (!dead)
        {
            dead = true;
            animPlayer.SetTrigger("Death");
            yield return new WaitForSeconds(0.5f);
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            playerCollider.isTrigger = true;
            Invoke("RestartGame", 2.5f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Fase1");
    }
}
