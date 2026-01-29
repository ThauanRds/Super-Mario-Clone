using System.Collections;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Rigidbody2D rbGoomba;
    public float speed = 2f;
    [SerializeField] Transform point1, point2;
    [SerializeField] LayerMask layer;
    [SerializeField] bool isColliding;

    public Animator animGoomba;
    BoxCollider2D colliderGoomba;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rbGoomba = GetComponent<Rigidbody2D>();

        animGoomba = GetComponent<Animator>();
        colliderGoomba = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rbGoomba.velocity = new Vector2(-speed, rbGoomba.velocity.y);

        isColliding = Physics2D.Linecast(point1.position, point2.position, layer);

        Debug.DrawLine(point1.position, point2.position, Color.red);

        if (isColliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (transform.position.y + 0.5f < collision.transform.position.y)
            {
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                animGoomba.SetTrigger("Death");
                speed = 0;
                Destroy(gameObject, 0.3f);
                colliderGoomba.enabled = false;
            }
            else
            {
                if (PlayerMovement.isGrow)
                {
                    PlayerMovement.isGrow = false;
                }
                else
                {
                    FindObjectOfType<PlayerMovement>().Death();

                    Goomba[] goomba = FindObjectsOfType<Goomba>();

                    for (int i = 0; i < goomba.Length; i++)
                    {
                        goomba[i].speed = 0;
                        goomba[i].animGoomba.speed = 0;
                    }
                }
            }
        }
    }
}
