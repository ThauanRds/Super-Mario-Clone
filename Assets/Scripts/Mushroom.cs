using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] bool moveLeft = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveLeft)
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.right * speed * Time.deltaTime);       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (!moveLeft)
                moveLeft = true;
            else
                moveLeft = false;
        }

        if(collision.CompareTag("Player"))
        {
            PlayerMovement.isGrow = true;
            Destroy(gameObject);
        }
            
    }
}
