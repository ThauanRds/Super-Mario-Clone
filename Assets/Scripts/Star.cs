using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Star : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] bool moveRight = true;
    [SerializeField] float jumpForce = 5f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
            transform.Translate(7 * Time.deltaTime, 0, 0);
        else
            transform.Translate(-7 * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 6:

                if (moveRight)
                    moveRight = false;
                else
                    moveRight = true;
                break;

            case 9:
                PlayerMovement.powerUp = true;
                Destroy(gameObject);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
