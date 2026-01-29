using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float minX, maxX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.x >= transform.position.x)
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), 0, transform.position.z);

    }
}
