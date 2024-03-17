using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float speed = 15f;
    [SerializeField] private float lifespan = 1.1f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifespan);
    }

    public void shoot(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Background")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        WrapAround();
    }

    void WrapAround()
    {
        Vector3 newPosition = transform.position;
        //left side of the screen
        if (transform.position.x < -9.6f)
        {
            //too far to the left, come back in the right side
            newPosition.x += 19.2f;
        }

        //right side of the screen
        if (transform.position.x > 9.6f)
        {
            //too far to the right, come back in the left side
            newPosition.x -= 19.2f;
        }

        //top of the screen
        if (transform.position.y > 5.4f)
        {
            //too far to the top, come back in the bottom side
            newPosition.y -= 10.8f;
        }

        //bottom of the screen
        if (transform.position.y < -5.4f)
        {
            //too far to the bottom, come back in the top side
            newPosition.y += 10.8f;
        }

        transform.position = newPosition;
    }
}
