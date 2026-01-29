using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class Flower : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player != null)
        {
            if (!player.isFlower && !PlayerMovement.isGrow)
            {
                PlayerMovement.isGrow = true;
            }
            else
            {
                player.isFlower = true;
            }

            Destroy(gameObject);
        }
    }
}
