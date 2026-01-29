using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D rbBall;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float force;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbBall = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
            rbBall.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        else if (collision.gameObject.layer != 3)
        {
            GameObject tempExplosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(tempExplosion, 0.25f);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 7)
        {
            Vector3 tempRotation = collision.transform.localEulerAngles;
            tempRotation.z = 180f;
            collision.gameObject.transform.localEulerAngles = tempRotation;

            collision.gameObject.GetComponent<Goomba>().animGoomba.speed = 0;
            collision.gameObject.GetComponent<Goomba>().rbGoomba.AddForce(new Vector2(5, 7), ForceMode2D.Impulse);

            Collider2D[] goombaCollider = collision.gameObject.GetComponents<Collider2D>();
            foreach (Collider2D c in goombaCollider)
            {
                c.enabled = false;
            }
        }
    }
}
