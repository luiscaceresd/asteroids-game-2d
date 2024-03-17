using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    Rigidbody2D rb;
    SpriteRenderer sr;
    PolygonCollider2D pc;
    float speed = 2f;
    [SerializeField] private Explode explode;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pc = GetComponent<PolygonCollider2D>();
    }
    
    public void kick(float mass, Vector2 direction)
    {
        //choosing a random sprite
        sr.sprite = sprites[Random.Range(0, sprites.Length)];

        //build the polygon collider
        List<Vector2> path = new List<Vector2>();
        sr.sprite.GetPhysicsShape(0, path);
        pc.SetPath(0, path.ToArray());
        pc.enabled = true;

        //set the mass and size of the asteroid
        rb.mass = mass;
        float width = Random.Range(0.75f, 1.33f);
        float height = 1 / width;
        transform.localScale = new Vector2(width, height) * mass;

        //move and rotate the asteroid
        rb.velocity = direction.normalized * speed;
        rb.AddTorque(Random.Range(-1f, 1f));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Background")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Debug.Log("Asteroid Split");
            //split the asteroid if it's large enough
            if (rb.mass > 0.7f)
            {
                split();
                split();                
            }
            //randomly choose a rotation
            float angle = Random.Range(0, 360f);
            //make a rotation transform
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Explode newExplode = Instantiate(explode, transform.position, rotation);

            Destroy(gameObject);
        }
    }

    private void split()
    {
        //set the new asteroid position to be the same as the old one,
        //but with a slight offset
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        //create a new asteroid 1/2 the size of the old one
        Asteroid smallAsteroid = Instantiate(this, position, transform.rotation);
        Vector2 direction = Random.insideUnitCircle;
        
        //set the mass and size of the new asteroid
        float mass = rb.mass / 2;
        smallAsteroid.kick(mass, direction);
    }
}
