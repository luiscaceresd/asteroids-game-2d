using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Bullet bullet;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool forceOn = false; //are we thrusting?
    [SerializeField] float forceAmount = 10.0f;
    float torqueDirection = 0.0f; //rotation direction, either -1, 0, or 1
    [SerializeField] float torqueAmount = 0.5f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //when spacebar is down, shoot a bullet
        if (Input.GetKey(KeyCode.Space))
        {
            //create a bullet
            Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            //shoot it in the direction the ship is facing
            newBullet.shoot(transform.up);
        }

        //on W key press apply thrusting force
        // are we pressing W?
        forceOn = Input.GetKey(KeyCode.W);
        // set animator isThrusting to true if we are pressing W
        animator.SetBool("isThrusting", forceOn);

        //on S key down rotate 180 degrees
        if (Input.GetKeyDown(KeyCode.S))
        {
            //rotate 180 degrees
            transform.RotateAround(transform.position, new Vector3(0, 0, 1), 180.0f);
        }

        //on A key rotate left, D key rotate right
        if (Input.GetKey(KeyCode.A))
        {
            torqueDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            torqueDirection = -1.0f;
        }
        else
        {
            torqueDirection = 0.0f;
        }

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

    private void FixedUpdate()
    {

        //apply force if we are thrusting
        if (forceOn)
        {
            rb.AddForce(transform.up * forceAmount);
        }

        //apply torque if we are rotating
        if (torqueDirection != 0)
        {
            rb.AddTorque(torqueDirection * torqueAmount);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if we collide with an asteroid, destroy the ship
        if (collision.gameObject.tag == "Asteroid")
        {
            // create an explosion
            // randomly choose a rotation
            float angle = Random.Range(0, 360f);
            //make a rotation transform
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject newExplode = Instantiate(explosion, transform.position, rotation);

            // move us off the screen
            transform.position = new Vector3(40000f, 40000f);
            // reset all velocities to 0
            rb.velocity = new Vector2(0f, 0f);
            rb.angularVelocity = 0.0f;
            // move the ship to the ignore layer
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            // call the reset method after 3 seconds
            Invoke("Reset", 3.0f);
        }
    }

    private void Reset()
    {
        // change the ship so it only shows some color and is semi-transparent
        sr.color = new Color(1.0f, 0f, 1.0f, 0.2f);
        // move the ship back to the center of the screen
        // and reset the rotation
        transform.position = new Vector2(0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        // call the method to turn everything back on for regular play
        Invoke("TurnOnCollissions", 3.0f);
    }

    private void TurnOnCollissions()
    {
        // change the ship back to full color and full opacity
        sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        // move the ship back to the regular layer
        gameObject.layer = LayerMask.NameToLayer("Ship");
    }   
}
